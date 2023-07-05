namespace AspNetCoreIdentityApp.web.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail);
    }
}
