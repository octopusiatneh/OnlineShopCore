using Microsoft.AspNetCore.Identity;
using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopCore.Data.Entities
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser<Guid>, IDateTracking, ISwitchable
    {
        public AppUser() { }
        public AppUser(Guid id, string fullName, string address, string userName,
            string email, string phoneNumber, string avatar,string province, int districtID, string wardCode, Status status)
        {
            Id = id;
            FullName = fullName;
            Address = address;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Avatar = avatar;
            Province = province;
            DistrictID = districtID;
            WardCode = wardCode;
            Status = status;
        }

        public string FullName { get; set; }

        public string Address { get; set; }

        public DateTime? BirthDay { set; get; }

        public decimal Balance { get; set; }

        public string Avatar { get; set; }
        public string Province { get; set; }

        public int? DistrictID { get; set; }

        public string WardCode { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Status Status { get; set; }
    }
}
