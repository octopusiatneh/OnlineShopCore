using AutoMapper;
using AutoMapper.QueryableExtensions;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;
using OnlineShopCore.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineShopCore.Application.Implementation
{
    public class PublisherService : IPublisherService
    {
        private IPublisherCompanyRepository _publisherRepository;
        private IUnitOfWork _unitOfWork;

        public PublisherService(IPublisherCompanyRepository publisherRepository,
            IUnitOfWork unitOfWork)
        {
            _publisherRepository = publisherRepository;
            _unitOfWork = unitOfWork;
        }
        public PublisherViewModel Add(PublisherViewModel publisherVm)
        {
            var publisher = Mapper.Map<PublisherViewModel, Publisher>(publisherVm);
            _publisherRepository.Add(publisher);
            return publisherVm;
        }

        public void Delete(int id)
        {
            _publisherRepository.Remove(id);
        }

        public List<PublisherViewModel> GetAll()
        {
            return _publisherRepository.FindAll().OrderBy(x => x.Id)
                 .ProjectTo<PublisherViewModel>().ToList();
        }

        public PublisherViewModel GetById(int id)
        {
            return Mapper.Map<Publisher, PublisherViewModel>(_publisherRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(PublisherViewModel publisherVm)
        {
            var publisher = Mapper.Map<PublisherViewModel, Publisher>(publisherVm);
            _publisherRepository.Update(publisher);
        }
    }
}
