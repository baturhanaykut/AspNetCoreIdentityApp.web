using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.web.Areas.Admin.Models
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Role isim alanı boş bırakılamaz.")]
        [Display(Name = "Role İsim Alanı :")]
        public string Name { get; set; }
    }
}
