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
    public class AuthorService : IAuthorService
    {
        private IAuthorRepository _authorRepository;
        private IUnitOfWork _unitOfWork;

        public AuthorService(IAuthorRepository authorRepository,
            IUnitOfWork unitOfWork)
        {
            _authorRepository = authorRepository;
            _unitOfWork = unitOfWork;
        }
        public AuthorViewModel Add(AuthorViewModel authorVm)
        {
            var author = Mapper.Map<AuthorViewModel, Author>(authorVm);
            _authorRepository.Add(author);
            return authorVm;
        }

        public void Delete(int id)
        {
            _authorRepository.Remove(id);
        }

        public List<AuthorViewModel> GetAll()
        {
            return _authorRepository.FindAll().ProjectTo<AuthorViewModel>().ToList();
        }

        public AuthorViewModel GetById(int id)
        {
            return Mapper.Map<Author, AuthorViewModel>(_authorRepository.FindById(id));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(AuthorViewModel authorVm)
        {
            var author = Mapper.Map<AuthorViewModel, Author>(authorVm);
            _authorRepository.Update(author);
        }
    }
}
