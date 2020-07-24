using AutoMapper;
using AutoMapper.QueryableExtensions;
using OfficeOpenXml;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Common;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Application.ViewModels.Utilities;
using OnlineShopCore.Data.EF;
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
        readonly IProductImageRepository _productImageRepository;
        readonly IPromotionRepository _promotionReposirory;
        readonly IPromotionDetailRepository _promotionDetailRepository;
        readonly AppDbContext _context;
        readonly IUnitOfWork _unitOfWork;
        public ProductService(IProductRepository productRepository, IProductImageRepository productImageRepository, IPromotionRepository promotionReposirory, IPromotionDetailRepository promotionDetailRepository, AppDbContext context, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _promotionReposirory = promotionReposirory;
            _promotionDetailRepository = promotionDetailRepository;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public ProductViewModel Add(ProductViewModel productVm)
        {
            var product = Mapper.Map<ProductViewModel, Product>(productVm);
            _productRepository.Add(product);
            return productVm;
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

        public void Delete(int id)
        {
            _productRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
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
                case "low-to-high":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "high-to-low":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                case "price-0-50":
                    query = _productRepository.FindAll(x => x.Price >= 0 && x.Price <= 50000 && x.Status == Status.Active);
                    break;
                case "price-50-100":
                    query = _productRepository.FindAll(x => x.Price >= 50000 && x.Price <= 100000 && x.Status == Status.Active);
                    break;
                case "price-100-200":
                    query = _productRepository.FindAll(x => x.Price >= 100000 && x.Price <= 200000 && x.Status == Status.Active);
                    break;
                case "price-200-500":
                    query = _productRepository.FindAll(x => x.Price >= 200000 && x.Price <= 500000 && x.Status == Status.Active);
                    break;
                case "price-500":
                    query = _productRepository.FindAll(x => x.Price >= 500000 && x.Status == Status.Active);
                    break;
                default:
                    query = _productRepository.FindAll(x => x.Status == Status.Active);
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

        public List<ProductViewModel> GetAll()
        {
            //get all active product
            return _productRepository.FindAll(x => x.Status == Status.Active).ProjectTo<ProductViewModel>().ToList();

            //get all product(active & inactive)
            //return _productRepository.FindAll().ProjectTo<ProductViewModel>().ToList();
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

        public List<ProductViewModel> GetAvailableProductForPromotion(DateTime dateStart)
        {
            var listPromo = _promotionReposirory.FindAll(x => x.Status == Status.Active).ToList();
            var productDbSet = _productRepository.FindAll(x => x.Status == Status.Active).ToList();
            var listUnavailableProduct = new List<Product>();

            foreach (var promo in listPromo)
            {
                var listPromoDetail = _promotionDetailRepository.FindAll(x => x.PromotionId == promo.Id);
                if (promo.DateEnd > dateStart)
                {              
                    foreach (var item in listPromoDetail)
                    {
                        var product = _productRepository.FindById(item.ProductId);
                        listUnavailableProduct.Add(product);
                    }
                }
            }

            productDbSet.RemoveAll(t => listUnavailableProduct.Contains(t));
            var products = Mapper.Map<List<Product>, List<ProductViewModel>>(productDbSet);
            return products;
        }

        public ProductViewModel GetById(int id)
        {
            return Mapper.Map<Product, ProductViewModel>(_productRepository.FindById(id));
        }

        public List<ProductViewModel> GetByName(string keyword)
        {
            var product = _productRepository.FindAll(x => x.Name.Contains(keyword));
            return product.ProjectTo<ProductViewModel>().ToList();
        }

        public List<ProductViewModel> GetHomeProduct(int top)
        {
            return _productRepository.FindAll(x => x.Status == Status.Active && x.HomeFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(top)
                .ProjectTo<ProductViewModel>()
                .ToList();
        }

        public List<ProductViewModel> GetHotProduct(int top)
        {
            return _productRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(top)
                .ProjectTo<ProductViewModel>()
                .ToList();
        }

        public List<ProductImageViewModel> GetImages(int productId)
        {
            return _productImageRepository.FindAll(x => x.ProductId == productId)
                .ProjectTo<ProductImageViewModel>().ToList();
        }

        public List<ProductViewModel> GetLastest(int top)
        {
            return _productRepository.FindAll(x => x.Status == Status.Active).OrderByDescending(x => x.DateCreated)
                .Take(top).ProjectTo<ProductViewModel>().ToList();
        }

        public List<PromotionViewModel> GetPromotion()
        {
            return _promotionReposirory.FindAll(x => x.Status == Status.Active && x.DateStart < DateTime.Now)
                .ProjectTo<PromotionViewModel>()
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

        public void ImportExcel(string filePath, int categoryId, int authorId, int publisherId)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                Product product;
                for (int i = workSheet.Dimension.Start.Row + 1; i < workSheet.Dimension.End.Row; i++)
                {
                    if (workSheet.Cells[i,1].Value == null)
                        break;
                    product = new Product
                    {
                        CategoryId = categoryId,
                        AuthorId = authorId,
                        PublisherId = publisherId,
                        Name = workSheet.Cells[i, 1].Value.ToString(),
                        Description = null,
                        SeoAlias = workSheet.Cells[i,7].Value.ToString()
                    };

                    float.TryParse(workSheet.Cells[i, 2].Value.ToString(), out var width);
                    product.Width = width;

                    float.TryParse(workSheet.Cells[i, 3].Value.ToString(), out var height);
                    product.Height = height;

                    int.TryParse(workSheet.Cells[i, 4].Value.ToString(), out var totalPage);
                    product.TotalPage = totalPage;

                    decimal.TryParse(workSheet.Cells[i, 5].Value.ToString(), out var price);
                    product.Price = price;

                    product.Content = workSheet.Cells[i, 6].Value.ToString();

                    bool.TryParse(workSheet.Cells[i, 8].Value.ToString(), out var homeFlag);
                    product.HomeFlag = homeFlag;

                    bool.TryParse(workSheet.Cells[i, 9].Value.ToString(), out var hotFlag);
                    product.HotFlag = hotFlag;
   
                    product.Status = Status.Active;
                    product.ViewCount = 0;
                    product.Image = "/uploaded/images/constraint/no_results_found.png";

                    _productRepository.Add(product);
                }
            }
        }

        public void IncreaseViewCount(int id)
        {
            _productRepository.FindById(id).ViewCount += 1;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductViewModel productVm)
        {
            var product = Mapper.Map<ProductViewModel, Product>(productVm);
            _productRepository.Update(product);
        }
    }
}