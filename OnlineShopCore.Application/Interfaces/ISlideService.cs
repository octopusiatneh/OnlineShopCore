using OnlineShopCore.Application.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.Interfaces
{
    public interface ISlideService : IDisposable
    {
        SlideViewModel Add(SlideViewModel slideVm);

        void Update(SlideViewModel slideVm);

        void Delete(int id);

        List<SlideViewModel> GetAll();

        SlideViewModel GetById(int id);

        List<SlideImageViewModel> GetImages(int productId);

        void Save();
    }
}
