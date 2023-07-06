using AspNetCoreIdentityApp.web.Models;

namespace AspNetCoreIdentityApp.web.ViewModels
{
    public class UserViewModel
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public DateTime? BirthDate { get; set; }

        public Gender? Gender { get; set; }

        public string? PictureUrl { get; set; }
    }
}
