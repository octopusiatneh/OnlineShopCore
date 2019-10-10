﻿using OnlineShopCore.Data.Entities;
using OnlineShopCore.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.EF.Repositories
{
    public class PublisherCompanyRepository : EFRepository<Publisher, int>, IPublisherCompanyRepository
    {
        AppDbContext _context;
        public PublisherCompanyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
