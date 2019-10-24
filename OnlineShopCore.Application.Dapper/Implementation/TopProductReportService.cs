using Dapper;
using Microsoft.Extensions.Configuration;
using OnlineShopCore.Application.Dapper.Intefaces;
using OnlineShopCore.Application.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopCore.Application.Dapper.Implementation
{
    public class TopProductReportService : ITopProductReportService
    {
        private readonly IConfiguration _configuration;

        public TopProductReportService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<TopProductReportViewModel>> GetReport()
        {

            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await sqlConnection.OpenAsync();
                try
                {
                    return await sqlConnection.QueryAsync<TopProductReportViewModel>(
                        "GetTopVisitProduct", commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
