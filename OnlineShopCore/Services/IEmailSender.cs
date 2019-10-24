using System.Threading.Tasks;

namespace OnlineShopCore.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
