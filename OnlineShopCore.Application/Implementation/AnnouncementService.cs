using AutoMapper.QueryableExtensions;
using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Data.Entities;
using OnlineShopCore.Infrastructure.Interfaces;
using OnlineShopCore.Utilities.Dtos;
using System.Linq;

namespace OnlineShopCore.Application.Implementation
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

        public PagedResult<AnnouncementViewModel> GetAllUnReadPaging(int pageIndex, int pageSize)
        {
            var query = from x in _announcementRepository.FindAll()
                        join y in _announcementBillRepository.FindAll()
                            on x.Id equals y.AnnouncementId
                            into xy
                        from annonBill in xy.DefaultIfEmpty()
                        where annonBill.HasRead == false
                        select x;
            int totalRow = query.Count();

            var model = query.OrderByDescending(x => x.DateCreated)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ProjectTo<AnnouncementViewModel>().ToList();
            var paginationSet = new PagedResult<AnnouncementViewModel>
            {
                Results = model,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }


        public bool MarkAsRead(string id)
        {
            bool result = false;
            var announ = _announcementBillRepository.FindSingle(x => x.AnnouncementId == id);
            if (announ == null)
            {
                _announcementBillRepository.Add(new AnnouncementBill(id, true));
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
