using AspNetCoreIdentityApp.web.Extensions;
using AspNetCoreIdentityApp.web.Models;
using AspNetCoreIdentityApp.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var userViewModel = new UserViewModel
            {
                Email = currentUser!.Email!,
                UserName = currentUser!.UserName!,
                PhoneNumber = currentUser!.PhoneNumber!

            };
            return View(userViewModel);
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();

            //return RedirectToAction("Index","Home");
        }

        public IActionResult PasswordChange()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser =await  _userManager.FindByNameAsync(User.Identity!.Name!);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.PasswordOld);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
                return View();
            }

            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser,request.PasswordOld,request.PasswordNew);

            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrolList(resultChangePassword.Errors.Select(x => x.Description).ToList());
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true,false);
            TempData["SuccessMessage"] = "Şifreniz başarı ile değiştirilmiştir.";

            return View();
        }
    }
}
