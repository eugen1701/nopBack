using Microsoft.IdentityModel.Tokens;
using NopApp.DAL.Repositories;
using NopApp.Models.ApiModels;
using NopApp.Models.DbModels;
using NopApp.Service.CustomExceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using NopApp.Service.Options;
using Microsoft.Extensions.Options;

namespace NopApp.Service
{
    public class EmailNotificationService
    {
        private EmailOptions _emailOptions;
        private SmtpClient _smtpClient;

        public EmailNotificationService(IOptions<EmailOptions> emailOptions)
        {
            this._emailOptions = emailOptions.Value;
            this._smtpClient = new SmtpClient
            {
                Host = _emailOptions.Host,
                Port = _emailOptions.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(this._emailOptions.Username, this._emailOptions.Password),
                Timeout = this._emailOptions.Timeout
            };
        }

        public void SendUserConfirmationEmail(User newUser)
        {
            MailMessage mail = BuildConfirmationEmail(newUser);

            _smtpClient.Send(mail);
        }

        public void SendManagerRejectedEmail(User newManager)
        {
            MailMessage mail = BuildRejectedMail(newManager);
            _smtpClient.Send(mail);
        }

        private MailMessage BuildConfirmationEmail(User newUser)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(this._emailOptions.From);
            mail.To.Add(new MailAddress(newUser.Email));

            string confirmationUrl = $"Please confirm your NopApp email address on {this._emailOptions.EmailConfirmationUrl}/{newUser.ConfirmationCode}";
            mail.Body = confirmationUrl;
            mail.Subject = "NopApp email confirmation";
            return mail;
        }

        private MailMessage BuildRejectedMail(User newUser)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(this._emailOptions.From);
            mail.To.Add(new MailAddress(newUser.Email));

            string confirmationUrl = "Your manager account registration has been rejected.";
            mail.Body = confirmationUrl;
            mail.Subject = "NopApp manager account request rejected";
            return mail;
        }
    }
}
