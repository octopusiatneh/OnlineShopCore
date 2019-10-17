using OnlineShopCore.Infrastructure.SharedKernel;
using System;

namespace OnlineShopCore.Data.Entities
{
    public class Logging : DomainEntity<int>
    {
        public DateTime ActionDate { get; set; }

        public string Message { get; set; }

        public string ActionType { get; set; }

        public Logging(DateTime actionDate, string message, string actionType)
        {
            ActionDate = actionDate;
            Message = message;
            ActionType = actionType;
        }
    }
}
