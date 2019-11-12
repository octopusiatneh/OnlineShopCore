using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Extensions;
using OnlineShopCore.Hubs;
using OnlineShopCore.Infrastructure.Interfaces;
using OnlineShopCore.Models;
using OnlineShopCore.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopCore.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IRepository<Announcement, string> _annouRepository;
        private readonly IRepository<AnnouncementBill, int> _annouBillRepository;
        private readonly IBillService _billService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _manager;

        public CartController(IProductService productService, IBillService billService, IRepository<Announcement, string> annouRepository,
            IRepository<AnnouncementBill, int> annouBillRepository,
            IUnitOfWork unitOfWork,
            UserManager<AppUser> manager,
           IHubContext<ChatHub> hubContext)
        {
            _manager = manager;
            _productService = productService;
            _billService = billService;
            _annouRepository = annouRepository;
            _annouBillRepository = annouBillRepository;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [Route("shopping-cart", Name = "Cart")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("checkout", Name = "Checkout")]
        public async Task<IActionResult> Checkout()
        {
            AppUser user = await GetCurrentUser();
            ViewData["UserInfo"] = user;

            var model = new CheckoutViewModel();
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session.Any(x => x.Quantity == 0))
            {
                return Redirect("/shopping-cart");
            }

            model.Carts = session;
            return View(model);
        }

        [Route("checkout", Name = "Checkout")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);

            if (ModelState.IsValid)
            {
                if (session != null)
                {
                    var details = new List<BillDetailViewModel>();
                    foreach (var item in session)
                    {
                        details.Add(new BillDetailViewModel()
                        {
                            Product = item.Product,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            ProductId = item.Product.Id
                        });
                    }
                    var billViewModel = new BillViewModel()
                    {
                        CustomerMobile = model.CustomerMobile,
                        PaymentMethod = model.PaymentMethod,
                        BillStatus = model.BillStatus,
                        CustomerAddress = model.CustomerAddress,
                        Province = model.Province,
                        DistrictID = model.DistrictID,
                        WardCode = model.WardCode,
                        CODAmount = model.CODAmount,
                        CustomerName = model.CustomerName,
                        CustomerMessage = model.CustomerMessage,
                        BillDetails = details
                    };
                    if (User.Identity.IsAuthenticated == true)
                    {
                        billViewModel.CustomerId = Guid.Parse(User.GetSpecificClaim("UserId"));
                    }

                    var notificationId = Guid.NewGuid().ToString();
                    var announcement = new AnnouncementViewModel()
                    {
                        Content = $"New bill from {billViewModel.CustomerName}",
                        DateCreated = DateTime.Now,
                        Id = notificationId,
                        Status = Status.Active,
                        Title = "New bill",
                    };
                    var announcementBills = new List<AnnouncementBillViewModel>()
                {
                    new AnnouncementBillViewModel(){AnnouncementId = notificationId,HasRead = false}
                };
                    try
                    {
                        _billService.Create(announcement, announcementBills, billViewModel);
                        await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);
                        _billService.Save();
                        ClearCart();
                        
                        //var content = await _viewRenderService.RenderToStringAsync("Cart/_BillMail", billViewModel);
                        //Send mail
                        //await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"], "New bill from Coza Store", content);
                        ViewData["Success"] = true;
                    }
                    catch (Exception ex)
                    {
                        ViewData["Success"] = false;
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }
            return View();
        }

        #region AJAX Request
        /// <summary>
        /// Get the current logged in user for quickly user information implement
        /// </summary>
        /// <returns></returns>
        private async Task<AppUser> GetCurrentUser()
        {
            return await _manager.GetUserAsync(HttpContext.User);
        }
        /// <summary>
        /// Get list item
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCart()
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session == null)
                session = new List<ShoppingCartViewModel>();
            return new OkObjectResult(session);
        }

        /// <summary>
        /// Remove all products in cart
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return new OkObjectResult("OK");
        }

        /// <summary>
        /// Add product to cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            //Get product detail
            var product = _productService.GetById(productId);

            //Get session with item list from cart
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                //Convert string to list object
                bool hasChanged = false;

                //Check exist with item product id
                if (session.Any(x => x.Product.Id == productId))
                {
                    foreach (var item in session)
                    {
                        //Update quantity for product if match product id
                        if (item.Product.Id == productId)
                        {
                            item.Quantity += quantity;
                            item.Price = product.PromotionPrice ?? product.Price;
                            hasChanged = true;
                        }
                    }
                }
                else
                {
                    session.Add(new ShoppingCartViewModel()
                    {
                        Product = product,
                        Quantity = quantity,
                        Price = product.PromotionPrice ?? product.Price
                    });
                    hasChanged = true;
                }

                //Update back to cart
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
            }
            else
            {
                //Add new cart
                var cart = new List<ShoppingCartViewModel>();
                cart.Add(new ShoppingCartViewModel()
                {
                    Product = product,
                    Quantity = quantity,
                    Price = product.PromotionPrice ?? product.Price
                });
                HttpContext.Session.Set(CommonConstants.CartSession, cart);
            }
            return new OkObjectResult(productId);
        }

        /// <summary>
        /// Remove a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IActionResult RemoveFromCart(int productId)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Update product quantity
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        var product = _productService.GetById(productId);
                        item.Product = product;
                        item.Quantity = quantity;
                        item.Price = product.PromotionPrice ?? product.Price;
                        hasChanged = true;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }
        #endregion AJAX Request
    }
}