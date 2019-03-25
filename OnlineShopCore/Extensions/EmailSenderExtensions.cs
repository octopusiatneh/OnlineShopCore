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
            string sHTML = File.ReadAllText(@"..\OnlineShopCore\wwwroot\templates\emailSendTemp.txt");
            sHTML = sHTML.Replace("id=\"veryImportant\" href=\"#\""
                , $"id=\"veryImportant\" href='{HtmlEncoder.Default.Encode(link)}' ");
            return emailSender.SendEmailAsync(email, "Welcome to CozaStore!!",
                sHTML);
        }

        public static Task SendEmailContactAsync(this IEmailSender emailSender, string email, string link)
        {
            string sHTML = File.ReadAllText(@"..\OnlineShopCore\wwwroot\templates\emailContactTemp.txt");
            sHTML = sHTML.Replace("id=\"veryImportant\" href=\"#\""
                , $"id=\"veryImportant\" href='{HtmlEncoder.Default.Encode(link)}' ");
            return emailSender.SendEmailAsync(email, "Welcome to CozaStore!!",
                sHTML);
        }
    }
}
