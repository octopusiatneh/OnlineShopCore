using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using OfficeOpenXml;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.Common;
using OnlineShopCore.Application.ViewModels.Product;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Hubs;
using OnlineShopCore.Infrastructure.Interfaces;
using OnlineShopCore.Utilities.Extensions;
using OnlineShopCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        [HttpPost]
        public IActionResult CreateOrderGHN(int serviceID, string customerName, string customerPhone, string customerAddress, int toDistrictID, string toWardCode, string customerMessage, int codAmount)
        {
            var url = "https://dev-online-gateway.ghn.vn/apiv3-api/api/v1/apiv3/CreateOrder";

            var requestBody = JsonConvert.SerializeObject(new
            {   //c6a869b80fbb4c2fb41079ffe864eda7
                token = "c6a869b80fbb4c2fb41079ffe864eda7",
                PaymentTypeID = 2,
                FromDistrictID = 1456,
                FromWardCode = "21502",
                ToDistrictID = toDistrictID,
                ToWardCode = toWardCode,
                ClientContactName = "Coza Store",
                ClientContactPhone = "0904285240",
                ClientAddress = "155 Sư Vạn Hạnh (nd), P.13, Q.10",
                CustomerName = customerName,
                CustomerPhone = customerPhone,
                ShippingAddress = customerAddress,
                CoDAmount = codAmount,
                NoteCode = "CHOXEMHANGKHONGTHU",
                ServiceID = serviceID,
                Weight = 1000,
                Length = 10,
                Width = 10,
                Height = 10,
                ReturnContactName = "Coza Store",
                ReturnContactPhone = "0904285240",
                ReturnAddress = "155 Sư Vạn Hạnh (nd), P.13, Q.10",
                ReturnDistrictID = 1456,
                ExternalReturnCode = "",
                Note = customerMessage

            });
            var data = new StringContent(requestBody, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            HttpResponseMessage response = client.PostAsync(url, data).Result;

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
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
                    Id = notificationId,
                    Status = Status.Active,
                    Title = "New bill",
                };
                var announcementBills = new List<AnnouncementBillViewModel>()
                {
                    new AnnouncementBillViewModel(){AnnouncementId = notificationId,HasRead = false}
                };

                _billService.Create(announcement, announcementBills, billVm);

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
                    worksheet.Cells[6, 1].Value = "Họ tên khách hàng: " + billDetail.CustomerName;
                    worksheet.Cells[7, 1].Value = "Số điện thoại: " + billDetail.CustomerMobile;
                    // Start Row for Detail Rows
                    int rowIndex = 10;

                    // load order details
                    var orderDetails = _billService.GetBillDetails(billId);
                    int count = 1;
                    foreach (var orderDetail in orderDetails)
                    {
                        // Cell 1, Order (thứ tự)
                        worksheet.Cells[rowIndex, 1].Value = count.ToString();
                        // Cell 2, Product name
                        worksheet.Cells[rowIndex, 2].Value = orderDetail.Product.Name;
                        // Cell 3, Quantity
                        worksheet.Cells[rowIndex, 3].Value = orderDetail.Quantity.ToString();
                        // Cell 4, Price
                        worksheet.Cells[rowIndex, 4].Value = orderDetail.Price.ToString("N0");
                        // Cell 5, Total
                        worksheet.Cells[rowIndex, 6].Value = (orderDetail.Price * orderDetail.Quantity).ToString("N0");
                        // Increment Row Counter
                        rowIndex++;
                        count++;
                    }
                    decimal total = (decimal)(orderDetails.Sum(x => x.Quantity * x.Price));
                    worksheet.Cells[rowIndex, 1].Value = "Tổng tiền (bằng số): ";
                    worksheet.Cells[rowIndex, 6].Value = total.ToString("N0");
                    worksheet.Cells[rowIndex, 6].Style.Font.Bold = true;
                    rowIndex++;
                    var numberWord = TextHelper.ToString(total);
                    worksheet.Cells[rowIndex, 1].Value = "Tổng tiền (bằng chữ): ";
                    worksheet.Cells[rowIndex, 2].Value = numberWord;

                    var billDate = billDetail.DateCreated;
                    worksheet.Cells[5, 2].Value = "Ngày " + billDate.Day + " tháng " + billDate.Month + " năm " + billDate.Year;

                    package.SaveAs(file); //Save the workbook.
                }
            }
            return new OkObjectResult(url);
        }
    }
}