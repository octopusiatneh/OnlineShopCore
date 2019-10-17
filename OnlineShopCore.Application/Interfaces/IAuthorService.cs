using OnlineShopCore.Application.ViewModels;
using System.Collections.Generic;

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
