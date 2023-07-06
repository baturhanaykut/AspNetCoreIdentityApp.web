using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.web.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Required(ErrorMessage = "Kullanıcı Email alanı boş bırakılamaz.")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = null!;


    }
}
