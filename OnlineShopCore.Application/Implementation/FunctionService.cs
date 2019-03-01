using OnlineShopCore.Application.Interfaces;
using OnlineShopCore.Application.ViewModels.System;
using OnlineShopCore.Data.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopCore.Application.Implementation
{
    public class FunctionService : IFunctionService
    {
        IFunctionRepository _functionRepository;
        public FunctionService(IFunctionRepository functionRepository)
        {
            _functionRepository = functionRepository;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task<List<FunctionViewModel>> GetAll()
        {
            return _functionRepository.FindAll().ProjectTo<FunctionViewModel>().ToListAsync();
        }

        public List<FunctionViewModel> GetAllByPermission(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
