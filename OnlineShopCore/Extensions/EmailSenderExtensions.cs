using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using OnlineShopCore.Services;

namespace OnlineShopCore.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            string sHTML = File.ReadAllText(@"D:\OnlineShop with ASP. NET Core\OnlineShopCore\OnlineShopCore\wwwroot\templates\emailSendTemp.txt");
            sHTML = sHTML.Replace("id=\"veryImportant\" href=\"#\""
                , $"id=\"veryImportant\" href='{HtmlEncoder.Default.Encode(link)}' ");
            return emailSender.SendEmailAsync(email, "Welcome to CozaStore!!",
                sHTML);
        }
    }
}
