﻿using OnlineShopCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopCore.Application.ViewModels.Product
{
    public class BillViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerName { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerAddress { set; get; }

        [Required]
        public int? ServiceID { get; set; }

        public string Province { get; set; }

        [Required]
        public int? DistrictID { get; set; }

        [Required]
        public string WardCode { get; set; }

        [Required]
        public int? CODAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string CustomerMobile { set; get; }

        [MaxLength(256)]
        public string CustomerMessage { set; get; }

        public PaymentMethod PaymentMethod { set; get; }

        public BillStatus BillStatus { set; get; }

        public DateTime DateCreated { set; get; }

        public DateTime DateModified { set; get; }

        public Status Status { set; get; } = Status.Active;

        public Guid? CustomerId { set; get; }

        public List<BillDetailViewModel> BillDetails { set; get; }
    }
}
