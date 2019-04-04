using AutoMapper;
using AutoMapper.QueryableExtensions;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Utilities;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.IRepositories;
using OnlineShopCore.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineShopCore.Application.Implementation
{
    public class SlideService : ISlideService
    {
        ISlideRepository _slideRepository;
        IUnitOfWork _unitOfWork;
        ISlideImageRepository _slideImageRepository;

        public SlideService(ISlideRepository slideRepository, IUnitOfWork unitOfWork, ISlideImageRepository slideImageRepository)
        {
            _slideRepository = slideRepository;
            _unitOfWork = unitOfWork;
            _slideImageRepository = slideImageRepository;
        }

        public SlideViewModel Add(SlideViewModel slideVm)
        {
            var slide = Mapper.Map<SlideViewModel, Slide>(slideVm);
            slide.Status = Status.Active;
            _slideRepository.Add(slide);
            return slideVm;
        }

        public void Delete(int id)
        {
            _slideRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public List<SlideViewModel> GetAll()
        {
            return _slideRepository.FindAll().OrderBy(x => x.Id)
                 .ProjectTo<SlideViewModel>().ToList();
        }
        public List<SlideViewModel> GetSlide()
        {
            return _slideRepository.FindAll(x=> x.Status == Status.Active).OrderBy(x => x.Id)
                 .ProjectTo<SlideViewModel>().ToList();
        }

        public SlideViewModel GetById(int id)
        {
            return Mapper.Map<Slide, SlideViewModel>(_slideRepository.FindById(id));
        }

        public List<SlideImageViewModel> GetImages(int slideId)
        {
            return _slideImageRepository.FindAll(x => x.SlideId == slideId)
                  .ProjectTo<SlideImageViewModel>().ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(SlideViewModel slideVm)
        {
            var slide = Mapper.Map<SlideViewModel, Slide>(slideVm);
            _slideRepository.Update(slide);
        }

        
    }
}
