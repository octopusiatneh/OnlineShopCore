using OnlineShopCore.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Application.Interfaces
{
    public interface IAuthorService
    {
        AuthorViewModel Add(AuthorViewModel authorVm);

        void Update(AuthorViewModel authorVm);

        void Delete(int id);

        List<AuthorViewModel> GetAll();


        AuthorViewModel GetById(int id);

        void Save();
    }
}
