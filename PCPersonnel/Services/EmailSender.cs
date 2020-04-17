using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace PCPersonnel.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender()
        {
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await Task.Yield();
        }
    }
}
