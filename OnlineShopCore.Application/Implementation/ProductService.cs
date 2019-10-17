using AutoMapper;
using AutoMapper.QueryableExtensions;
using OfficeOpenXml;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Common;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.IRepositories;
using OnlineShopCore.Infrastructure.Interfaces;
using OnlineShopCore.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OnlineShopCore.Application.Implementation
{
    public class ProductService : IProductService
    {
        readonly IProductRepository _productRepository;
        readonly IUnitOfWork _unitOfWork;
        readonly IProductImageRepository _productImageRepository;


        public ProductService(IProductRepository productRepository, IProductImageRepository productImageRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _unitOfWork = unitOfWork;
        }

        public ProductViewModel Add(ProductViewModel productVm)
        {
            var product = Mapper.Map<ProductViewModel, Product>(productVm);
            _productRepository.Add(product);
            return productVm;
        }

        public void Update(ProductViewModel productVm)
        {
            var product = Mapper.Map<ProductViewModel, Product>(productVm);
            _productRepository.Update(product);
        }

        public void Delete(int id)
        {
            _productRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ProductViewModel> GetAll()
        {
            return _productRepository.FindAll(x => x.ProductCategory).ProjectTo<ProductViewModel>().ToList();
        }

        public ProductViewModel GetById(int id)
        {
            return Mapper.Map<Product, ProductViewModel>(_productRepository.FindById(id));
        }

        public void ImportExcel(string filePath, int categoryId, int authorId, int publisherId)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                Product product;
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    product = new Product
                    {
                        CategoryId = categoryId,
                        AuthorId = authorId,
                        PublisherId = publisherId,
                        Name = workSheet.Cells[i, 1].Value.ToString(),
                        Description = workSheet.Cells[i, 2].Value.ToString()
                    };

                    decimal.TryParse(workSheet.Cells[i, 3].Value.ToString(), out var price);
                    product.Price = price;
                    decimal.TryParse(workSheet.Cells[i, 4].Value.ToString(), out var promotionPrice);

                    product.PromotionPrice = promotionPrice;
                    product.Content = workSheet.Cells[i, 5].Value.ToString();  

                    bool.TryParse(workSheet.Cells[i, 8].Value.ToString(), out var hotFlag);
                    product.HotFlag = hotFlag;

                    bool.TryParse(workSheet.Cells[i, 9].Value.ToString(), out var homeFlag);
                    product.HomeFlag = homeFlag;

                    product.Status = Status.Active;

                    _productRepository.Add(product);
                }
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<ProductImageViewModel> GetImages(int productId)
        {
            return _productImageRepository.FindAll(x => x.ProductId == productId)
                .ProjectTo<ProductImageViewModel>().ToList();
        }

        public void AddImages(int productId, string[] images)
        {
            _productImageRepository.RemoveMultiple(_productImageRepository.FindAll(x => x.ProductId == productId).ToList());
            foreach (var image in images)
            {
                _productImageRepository.Add(new ProductImage()
                {
                    Path = image,
                    ProductId = productId,
                    Caption = string.Empty
                });
            }

        }

        public PagedResult<ProductViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _productRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword) || x.Author.AuthorName.Contains(keyword));

            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize).Take(pageSize);

            var data = query.ProjectTo<ProductViewModel>().ToList();

            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public PagedResult<ProductViewModel> Filter(string filter, int page, int pageSize)
        {
            var query = _productRepository.FindAll(x => x.Status == Status.Active);
            switch (filter)
            {
                case "lastest":
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
                case "discount":
                    query = _productRepository.FindAll(x => x.PromotionPrice.HasValue);
                    break;
                case "lowtohigh":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "hightolow":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                case "price-0-50":
                    query = _productRepository.FindAll(x => x.Price >= 0 && x.Price <= 50);
                    break;
                case "price-50-100":
                    query = _productRepository.FindAll(x => x.Price >= 50 && x.Price <= 100);
                    break;
                case "price-100-150":
                    query = _productRepository.FindAll(x => x.Price >= 100 && x.Price <= 150);
                    break;
                case "price-150-200":
                    query = _productRepository.FindAll(x => x.Price >= 150 && x.Price <= 200);
                    break;
                case "price-200":
                    query = _productRepository.FindAll(x => x.Price >= 200);
                    break;
                default:
                    query = _productRepository.FindAll();
                    break;
            }

            int totalRow = query.Count();
            query = query.OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize).Take(pageSize);
            var data = query.ProjectTo<ProductViewModel>().ToList();
            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public List<ProductViewModel> GetLastest(int top)
        {
            return _productRepository.FindAll(x => x.Status == Status.Active).OrderByDescending(x => x.DateCreated)
                .Take(top).ProjectTo<ProductViewModel>().ToList();
        }

        public List<ProductViewModel> GetHotProduct(int top)
        {
            return _productRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(top)
                .ProjectTo<ProductViewModel>()
                .ToList();
        }

        public List<ProductViewModel> GetHomeProduct(int top)
        {
            return _productRepository.FindAll(x => x.Status == Status.Active && x.HomeFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(top)
                .ProjectTo<ProductViewModel>()
                .ToList();
        }

        public List<ProductViewModel> GetRelatedProducts(int id, int top)
        {
            var product = _productRepository.FindById(id);
            return _productRepository.FindAll(x => x.Status == Status.Active
                && x.Id != id && x.CategoryId == product.CategoryId)
            .OrderByDescending(x => x.DateCreated)
            .Take(top)
            .ProjectTo<ProductViewModel>()
            .ToList();
        }     

        public List<ProductViewModel> GetByName(string keyword)
        {
            var product = _productRepository.FindAll(x => x.Name.Contains(keyword));
            return product.ProjectTo<ProductViewModel>().ToList();
        }


    }
}