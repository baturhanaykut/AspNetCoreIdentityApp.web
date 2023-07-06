using AspNetCoreIdentityApp.web.OptionsModel;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentityApp.web.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail)
        {
            var smtpClient = new SmtpClient
            {
                Host = _emailSettings.Host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password),
                EnableSsl = true
            };
            MailMessage mailMessage = new()
            {
                From = new MailAddress(_emailSettings.Email),
                Subject = "Localhost | Şifre sıfırlama linki",
                Body = @$"
                <h4>Şifrenizi yenilemek için aşağıdaki linke tıklayanız. </h4>
                <p><a href='{resetPasswordEmailLink}'>Şifre yenileme link</a></p>",
                IsBodyHtml = true,
                To={ToEmail}
                
            };
            //mailMessage.To.Add(ToEmail);
            //mailMessage.Subject = "Localhost | Şifre sıfırlama linki";
            //mailMessage.Body = @$"
            //    <h4>Şifrenizi yenilemek için aşağıdaki linke tıklayanız. </h4>
            //    <p><a href='{resetPasswordEmailLink}'>Şifre yenileme link</a></p>";
            //mailMessage.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
