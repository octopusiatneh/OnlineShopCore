﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.IRepositories;
using OnlineShopCore.Infrastructure.Interfaces;
using OnlineShopCore.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OnlineShopCore.Application.Implementation
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _orderRepository;
        private readonly IBillDetailRepository _orderDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly IRepository<Announcement, string> _announRepository;
        private readonly IRepository<AnnouncementBill, int> _announBillRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BillService(IBillRepository orderRepository,
           IBillDetailRepository orderDetailRepository,
           IProductRepository productRepository,
           IRepository<Announcement, string> announRepository,
           IRepository<AnnouncementBill, int> announBillRepository,
           IUnitOfWork unitOfWork)
        {
            _announRepository = announRepository;
            _announBillRepository = announBillRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(AnnouncementViewModel announcementVm, List<AnnouncementBillViewModel> announcementBillVm, BillViewModel billVm)
        {
            var order = Mapper.Map<BillViewModel, Bill>(billVm);
            var orderDetails = Mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(billVm.BillDetails);
            foreach (var detail in orderDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                //detail.Price = product.Price;
            }
            var announcement = Mapper.Map<AnnouncementViewModel, Announcement>(announcementVm);
            _announRepository.Add(announcement);
            foreach (var item in announcementBillVm)
            {
                var i = Mapper.Map<AnnouncementBillViewModel, AnnouncementBill>(item);
                _announBillRepository.Add(i);
            }
            _unitOfWork.Commit();
            order.BillDetails = orderDetails;
            _orderRepository.Add(order);
        }

        public string GetBillStatus(int billId)
        {
            var order = _orderRepository.FindById(billId);
            switch(order.BillStatus)
            {
                case BillStatus.Cancelled:
                    return "Đã hủy";
                case BillStatus.Completed:
                    return "Đã giao hàng";
                case BillStatus.InProgress:
                    return "Đang giao hàng";
                case BillStatus.New:
                    return "Đang xử lý";
                case BillStatus.Returned:
                    return "Đã đổi trả";
                default:
                    return "something wrong here";
            }
        }

        public void Update(BillViewModel billVm)
        {
            //Mapping to order domain
            var order = Mapper.Map<BillViewModel, Bill>(billVm);

            //Get order Detail
            var newDetails = order.BillDetails;

            //new details added
            var addedDetails = newDetails.Where(x => x.Id == 0).ToList();

            //get updated details
            var updatedDetails = newDetails.Where(x => x.Id != 0).ToList();

            //Existed details
            var existedDetails = _orderDetailRepository.FindAll(x => x.BillId == billVm.Id);

            //Clear db
            order.BillDetails.Clear();

            foreach (var detail in updatedDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _orderDetailRepository.Update(detail);
            }

            foreach (var detail in addedDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _orderDetailRepository.Add(detail);
            }

            _orderDetailRepository.RemoveMultiple(existedDetails.Except(updatedDetails).ToList());

            _orderRepository.Update(order);
        }

        public void UpdateStatus(int billId, BillStatus status)
        {
            var order = _orderRepository.FindById(billId);
            order.BillStatus = status;
            Save();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword
           , int pageIndex, int pageSize)
        {
            var query = _orderRepository.FindAll();
            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated >= start);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated <= end);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.CustomerName.Contains(keyword) || x.CustomerMobile.Contains(keyword));
            }
            var totalRow = query.Count();
            var data = query.OrderByDescending(x => x.DateCreated)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<BillViewModel>()
                .ToList();
            return new PagedResult<BillViewModel>()
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
        }


        public BillViewModel GetDetail(int billId)
        {
            var bill = _orderRepository.FindById(billId);
            var billVm = Mapper.Map<Bill, BillViewModel>(bill);
            var billDetailVm = _orderDetailRepository.FindAll(x => x.BillId == billId).ProjectTo<BillDetailViewModel>().ToList();
            billVm.BillDetails = billDetailVm;
            return billVm;
        }

        public List<BillDetailViewModel> GetBillDetails(int billId)
        {
            return _orderDetailRepository
                .FindAll(x => x.BillId == billId, c => c.Bill, c => c.Product)
                .ProjectTo<BillDetailViewModel>().ToList();
        }

        public BillDetailViewModel CreateDetail(BillDetailViewModel billDetailVm)
        {
            var billDetail = Mapper.Map<BillDetailViewModel, BillDetail>(billDetailVm);
            _orderDetailRepository.Add(billDetail);
            return billDetailVm;
        }

        public void DeleteDetail(int productId, int billId, int colorId, int sizeId)
        {
            var detail = _orderDetailRepository.FindSingle(x => x.ProductId == productId && x.BillId == billId);
            _orderDetailRepository.Remove(detail);
        }

        public List<BillViewModel> GetAll()
        {
            return _orderRepository.FindAll().ProjectTo<BillViewModel>().ToList();
        }
    }
}
