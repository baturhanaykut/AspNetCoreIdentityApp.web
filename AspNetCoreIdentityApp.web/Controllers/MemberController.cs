﻿using AspNetCoreIdentityApp.web.Models;
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
    }
}