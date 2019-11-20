using AutoMapper;
using AutoMapper.QueryableExtensions;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Utilities;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.IRepositories;
using OnlineShopCore.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace OnlineShopCore.Application.Implementation
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IPromotionDetailRepository _promotionDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PromotionService(IPromotionRepository promotionRepository, IPromotionDetailRepository promotionDetailRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _promotionRepository = promotionRepository;
            _promotionDetailRepository = promotionDetailRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        public void Create(PromotionViewModel promoVm)
        {
            var promo = Mapper.Map<PromotionViewModel, Promotion>(promoVm);
            var promoDetails = Mapper.Map<List<PromotionDetailViewModel>, List<PromotionDetail>>(promoVm.PromotionDetails);
            foreach (var detail in promoDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                product.PromotionPrice = product.Price - (product.Price * detail.PromotionPercent / 100);
            }
            _promotionRepository.Add(promo);
        }

        public PromotionDetailViewModel CreateDetail(PromotionDetailViewModel promoDetailVm)
        {
            var promoDetail = Mapper.Map<PromotionDetailViewModel, PromotionDetail>(promoDetailVm);
            _promotionDetailRepository.Add(promoDetail);
            return promoDetailVm;
        }

        public void DeleteDetail(int productId, int promotionId)
        {
            var detail = _promotionDetailRepository.FindSingle(x => x.ProductId == productId && x.PromotionId == promotionId);
            _promotionDetailRepository.Remove(detail);
        }

        public List<PromotionViewModel> GetAll()
        {
            return _promotionRepository.FindAll().ProjectTo<PromotionViewModel>().ToList();
        }

        public PromotionViewModel GetDetail(int promotionId)
        {
            var promo = _promotionRepository.FindById(promotionId);
            var promoVm = Mapper.Map<Promotion, PromotionViewModel>(promo);
            var promoDetailVm = _promotionDetailRepository.FindAll(x => x.PromotionId == promotionId).ProjectTo<PromotionDetailViewModel>().ToList();
            promoVm.PromotionDetails = promoDetailVm;
            return promoVm;
        }

        public List<PromotionDetailViewModel> GetPromotionDetail(int promotionId)
        {
            return _promotionDetailRepository
                .FindAll(x => x.PromotionId == promotionId, c => c.Promotion, c => c.Product)
                .ProjectTo<PromotionDetailViewModel>().ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(PromotionViewModel promoVm)
        {
            //Mapping to order domain
            var promo = Mapper.Map<PromotionViewModel, Promotion>(promoVm);

            //Get order Detail
            var newDetails = promo.PromotionDetails;

            //new details added
            var addedDetails = newDetails.Where(x => x.Id == 0).ToList();

            //get updated details
            var updatedDetails = newDetails.Where(x => x.Id != 0).ToList();

            //Existed details
            var existedDetails = _promotionDetailRepository.FindAll(x => x.PromotionId == promoVm.Id);

            //Clear db
            promo.PromotionDetails.Clear();

            foreach (var detail in updatedDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _promotionDetailRepository.Update(detail);
            }

            foreach (var detail in addedDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _promotionDetailRepository.Add(detail);
            }

            _promotionDetailRepository.RemoveMultiple(existedDetails.Except(updatedDetails).ToList());

            _promotionRepository.Update(promo);
        }

        public void UpdateStatus(int promotionId)
        {
            var promo = _promotionRepository.FindById(promotionId);
            if (DateTime.Now >= promo.DateEnd)
            {
                promo.Status = Status.InActive;
                var promotDetails = _promotionDetailRepository.FindAll(x => x.PromotionId == promo.Id);

                foreach (var detail in promotDetails)
                {
                    var product = _productRepository.FindById(detail.ProductId);
                    product.PromotionPrice = null;
                    _productRepository.Update(product);
                }
                Save();
            }
        }
    }
}
