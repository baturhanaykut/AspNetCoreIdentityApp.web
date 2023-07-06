using AspNetCoreIdentityApp.web.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.web.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Ad alanı boş bırakılamaz.")]
        [Display(Name = "Kullanıcı Adı :")]
        public string UserName { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Required(ErrorMessage = "Kullanıcı Email alanı boş bırakılamaz.")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Kullanıcı Telefon alanı boş bırakılamaz.")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi :")]
        public DateTime? BirthDate { get; set; }
              
        [Display(Name = "Şehir :")]
        public string? City { get; set; }
        
        [Display(Name = "Profil Resmi :")]
        public IFormFile? Picture { get; set; }
        
        [Display(Name = "Cinsiyet :")]
        public Gender? Gender { get; set; }


    }
}
