﻿using OnlineShopCore.Application.ViewModels;
using System.Collections.Generic;

namespace OnlineShopCore.Application.Interfaces
{
    public interface IPublisherService
    {
        PublisherViewModel Add(PublisherViewModel PubliserVm);

        void Update(PublisherViewModel PubliserVm);

        void Delete(int id);

        List<PublisherViewModel> GetAll();

        PublisherViewModel GetById(int id);

        void Save();
    }
}
