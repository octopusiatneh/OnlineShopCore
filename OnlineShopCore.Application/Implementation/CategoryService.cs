using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.IRepositories;
using OnlineShopCore.Infrastructure.Interfaces;

namespace OnlineShopCore.Application.Implementation
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        private IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository CategoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = CategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public CategoryViewModel Add(CategoryViewModel CategoryVm)
        {
            var Category = Mapper.Map<CategoryViewModel, Category>(CategoryVm);
            _categoryRepository.Add(Category);
            return CategoryVm;
        }

        public void Delete(int id)
        {
            _categoryRepository.Remove(id);
        }

        public List<CategoryViewModel> GetAll()
        {
            return _categoryRepository.FindAll().OrderBy(x => x.DateCreated)
                 .ProjectTo<CategoryViewModel>().ToList();
        }
  
        public CategoryViewModel GetById(int id)
        {
            return Mapper.Map<Category, CategoryViewModel>(_categoryRepository.FindById(id));
        }    

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(CategoryViewModel CategoryVm)
        {
            var Category = Mapper.Map<CategoryViewModel, Category>(CategoryVm);
            _categoryRepository.Update(Category);
        }

        
    }
}
