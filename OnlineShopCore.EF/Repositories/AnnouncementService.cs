using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;
using OnlineShopCore.Infrastructure.Interfaces;
using OnlineShopCore.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class AnnouncementService : IAnnouncementService
    {
        private IRepository<Announcement, string> _announcementRepository;
        private IRepository<AnnouncementBill, int> _announcementBillRepository;

        private IUnitOfWork _unitOfWork;

        public AnnouncementService(IRepository<Announcement, string> announcementRepository,
            IRepository<AnnouncementBill, int> announcementBillRepository,
            IUnitOfWork unitOfWork)
        {
            _announcementBillRepository = announcementBillRepository;
            this._announcementRepository = announcementRepository;
            this._unitOfWork = unitOfWork;
        }

       

        public bool MarkAsRead(string id)
        {
            bool result = false;
            var announ = _announcementBillRepository.FindSingle(x => x.AnnouncementId == id );
            if (announ == null)
            {
                _announcementBillRepository.Add(new AnnouncementBill
                {
                    AnnouncementId = id,
                    HasRead = true
                });
                result = true;
            }
            else
            {
                if (announ.HasRead == false)
                {
                    announ.HasRead = true;
                    result = true;
                }

            }
            return result;
        }
    }
}
