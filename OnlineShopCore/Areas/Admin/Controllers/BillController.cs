using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShopCore.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Application.ViewModels.Product;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineShopCore.Application.ViewModels.Common;
using OnlineShopCore.Utilities.Extensions;
using System.IO;
using OfficeOpenXml;
using OnlineShopCore.Utilities.Helpers;
using Microsoft.AspNetCore.SignalR;
using OnlineShopCore.Hubs;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Infrastructure.Interfaces;
using OnlineShopCore.Data.Entities;

namespace OnlineShopCore.Areas.Admin.Controllers
{
    public class BillController : BaseController
    {
        private readonly IBillService _billService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IRepository<Announcement, string> _annouRepository;
        private readonly IRepository<AnnouncementBill, int> _annouBillRepository;

        private IUnitOfWork _unitOfWork;
        public BillController(IBillService billService, IHostingEnvironment hostingEnvironment,
            IRepository<Announcement, string> annouRepository,
            IRepository<AnnouncementBill, int> annouBillRepository,
            IUnitOfWork unitOfWork,
           IHubContext<ChatHub> hubContext)
        {
            _billService = billService;
            _hostingEnvironment = hostingEnvironment;
            _annouRepository = annouRepository;
            _annouBillRepository = annouBillRepository;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _billService.GetDetail(id);

            return new OkObjectResult(model);
        }
        [HttpGet]
        public IActionResult UpdateStatus(int billId, BillStatus status)
        {
            _billService.UpdateStatus(billId, status);

            return new OkResult();
        }
        [HttpGet]
        public IActionResult GetAllPaging(string startDate, string endDate, string keyword, int page, int pageSize)
        {
            var model = _billService.GetAllPaging(startDate, endDate, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _billService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(BillViewModel billVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (billVm.Id == 0)
            {
                var notificationId = Guid.NewGuid().ToString();
                var announcement = new AnnouncementViewModel()
                {
                    Content = $"New bill from {billVm.CustomerName}",
                    DateCreated = DateTime.Now,
                    Id= notificationId,
                    Status = Status.Active,
                    Title = "New bill",
                };
                var announcementBills = new List<AnnouncementBillViewModel>()
                {
                    new AnnouncementBillViewModel(){AnnouncementId = notificationId,HasRead = false}
                };

                _billService.Create(announcement, announcementBills,billVm);

                _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);
            }

            else
            {
                _billService.Update(billVm);
            }
            _billService.Save();
            return new OkObjectResult(billVm);
        }
        [HttpGet]
        public IActionResult GetPaymentMethod()
        {
            List<EnumModel> enums = ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return new OkObjectResult(enums);
        }

        [HttpGet]
        public IActionResult GetBillStatus()
        {
            List<EnumModel> enums = ((BillStatus[])Enum.GetValues(typeof(BillStatus)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return new OkObjectResult(enums);
        }

        [HttpGet]
        public IActionResult GetColors()
        {
            var colors = _billService.GetColors();
            return new OkObjectResult(colors);
        }

        [HttpGet]
        public IActionResult GetSizes()
        {
            var sizes = _billService.GetSizes();
            return new OkObjectResult(sizes);
        }
        [HttpPost]
        public IActionResult ExportExcel(int billId)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = $"Bill_{billId}.xlsx";
            // Template File
            string templateDocument = Path.Combine(sWebRootFolder, "templates", "BillTemplate.xlsx");

            string url = $"{Request.Scheme}://{Request.Host}/{"export-files"}/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, "export-files", sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["OrderDetail"];
                    // Data Acces, load order header data.
                    var billDetail = _billService.GetDetail(billId);

                    // Insert customer data into template
                    worksheet.Cells[4, 1].Value = "Customer Name: " + billDetail.CustomerName;
                    worksheet.Cells[5, 1].Value = "Address: " + billDetail.CustomerAddress;
                    worksheet.Cells[6, 1].Value = "Phone: " + billDetail.CustomerMobile;
                    // Start Row for Detail Rows
                    int rowIndex = 9;

                    // load order details
                    var orderDetails = _billService.GetBillDetails(billId);
                    int count = 1;
                    foreach (var orderDetail in orderDetails)
                    {
                        // Cell 1, Carton Count
                        worksheet.Cells[rowIndex, 1].Value = count.ToString();
                        // Cell 2, Order Number (Outline around columns 2-7 make it look like 1 column)
                        worksheet.Cells[rowIndex, 2].Value = orderDetail.Product.Name;
                        // Cell 8, Weight in LBS (convert KG to LBS, and rounding to whole number)
                        worksheet.Cells[rowIndex, 3].Value = orderDetail.Quantity.ToString();

                        worksheet.Cells[rowIndex, 4].Value = orderDetail.Price.ToString("N0");
                        worksheet.Cells[rowIndex, 5].Value = (orderDetail.Price * orderDetail.Quantity).ToString("N0");
                        // Increment Row Counter
                        rowIndex++;
                        count++;
                    }
                    decimal total = (decimal)(orderDetails.Sum(x => x.Quantity * x.Price));
                    worksheet.Cells[24, 5].Value = total.ToString("N0");

                    var numberWord = "Total amount (by word): " + TextHelper.ToString(total);
                    worksheet.Cells[26, 1].Value = numberWord;
                    var billDate = billDetail.DateCreated;
                    worksheet.Cells[28, 3].Value = billDate.Day + ", " + billDate.Month + ", " + billDate.Year;


                    package.SaveAs(file); //Save the workbook.
                }
            }
            return new OkObjectResult(url);
        }
    }
}