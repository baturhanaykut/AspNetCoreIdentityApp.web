using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Kullanıcı Şifre alanı boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre :")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifre aynı değildir.")]
        [Required(ErrorMessage = "Kullanıcı Şifre Tekrar alanı boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre Tekrar :")]
        public string PasswordConfirm { get; set; }
    }
}
