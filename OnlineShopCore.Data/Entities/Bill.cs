﻿using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.Interfaces;
using OnlineShopCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopCore.Data.Entities
{
    [Table("Bills")]
    public class Bill : DomainEntity<int>, ISwitchable, IDateTracking
    {
        public Bill() { }

        public Bill(string customerName, string customerAddress, string customerMobile, string customerMessage,
            BillStatus billStatus, PaymentMethod paymentMethod, Status status, Guid? customerId, DateTime dateCreated)
        {
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerMobile = customerMobile;
            CustomerMessage = customerMessage;
            BillStatus = billStatus;
            PaymentMethod = paymentMethod;
            Status = status;

            DateCreated = dateCreated;
            DateModified = DateTime.Now;
            CustomerId = customerId;
        }

        public Bill(int id, string customerName, string customerAddress, int? serviceID, string province, int? districtID, string wardCode, int? codAmount, string customerMobile, string customerMessage, BillStatus billStatus, PaymentMethod paymentMethod, Status status, Guid? customerId)
        {
            Id = id;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            ServiceID = serviceID;
            Province = province;
            DistrictID = districtID;
            WardCode = wardCode;
            CODAmount = codAmount;
            CustomerMobile = customerMobile;
            CustomerMessage = customerMessage;
            BillStatus = billStatus;
            PaymentMethod = paymentMethod;
            DateCreated = DateTime.Now;
            Status = status;
            CustomerId = customerId;
        }
        [Required]
        [MaxLength(256)]
        public string CustomerName { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerAddress { set; get; }

        [Required]
        public int? ServiceID { get; set; }

        [Required]
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

        [DefaultValue(Status.Active)]
        public Status Status { set; get; } = Status.Active;

        public Guid? CustomerId { set; get; }

        [ForeignKey("CustomerId")]
        public virtual AppUser User { set; get; }

        public virtual ICollection<BillDetail> BillDetails { set; get; }
    }
}