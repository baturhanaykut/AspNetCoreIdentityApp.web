using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.web.Localizations
{
    public class LocalizationsIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = "DuplicateUserName", Description = $"{userName} kullanıcı adı daha önce başka bir kullanıcı tarafından alınmıştır" };
            //return base.DuplicateUserName(userName);
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new() { Code = "DuplicateEmail", Description = $"Bu {email} daha önce başka bir kullanıcı tarafından alınmıştır" };
            //return base.DuplicateEmail(email);
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "PasswordTooShort", Description = "Şifre en az 6 karakter olmalıdır." };
            //return base.PasswordTooShort(length);
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new() { Code = "PasswordRequiresUpper", Description = "Lütfen bir adet büyük harf kullanınız." };
        }


    }
}
