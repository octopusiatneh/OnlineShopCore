﻿using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Utilities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShopCore.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddAsync(AppUserViewModel userVm);

        Task DeleteAsync(string id);

        Task<List<AppUserViewModel>> GetAllAsync();

        PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppUserViewModel> GetById(string id);


        Task UpdateAsync(AppUserViewModel userVm);
    }
}
