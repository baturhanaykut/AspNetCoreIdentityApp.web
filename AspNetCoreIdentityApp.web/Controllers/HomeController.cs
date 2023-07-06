using AspNetCoreIdentityApp.web.Models;
using AspNetCoreIdentityApp.web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityApp.web.Extensions;
using AspNetCoreIdentityApp.web.Services;

namespace AspNetCoreIdentityApp.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignUp()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityResult = await _userManager.CreateAsync(new() { UserName = request.UserName, PhoneNumber = request.Phone, Email = request.Email }, request.PasswordConfirm);

            if (identityResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıylar gerçekleşmiştir.";
                return RedirectToAction(nameof(HomeController.SignUp));
            }

            ModelState.AddModelErrolList(identityResult.Errors.Select(x => x.Description).ToList());

            //foreach (IdentityError item in identityResult.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, item.Description);
            //}

            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            returnUrl ??= Url.Action("Index", "Home");

            var hasUser = await _userManager.FindByEmailAsync(model.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre yanlış");
                return View();
            }


            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl!);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrolList(new List<string>() { "3 Dakika boyunca giriş yapamazsınız" });
                return View();
            }


            ModelState.AddModelErrolList(new List<string>() { "Email veya şifre yanlış", $"Başarısız giriş sayısı{await _userManager.GetAccessFailedCountAsync(hasUser)}" });


            return View();
        }



        public IActionResult ForgetPassword()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            var hasUser = await _userManager.FindByEmailAsync(model.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(String.Empty, "Bu email adresinde sahip kullanıcı bulunamamıştır");
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userID = hasUser.Id, Token = passwordResetToken },HttpContext.Request.Scheme);


            //örnek link https://localhost:7213?userId=12213&token=asdasdasfdasd

            await _emailService.SendResetPasswordEmail(passwordResetLink!, hasUser.Email!);

            TempData["SuccessMessage"] = "Şifre yenileme linki e-posta adresinize gönderilmiştir.";
            return RedirectToAction(nameof(ForgetPassword));
        }


        public IActionResult ResetPassword(string userId, string token) 
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId == null || token == null)
            {
                throw new Exception("Bir hata meydana geldi");
            }

            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);

            if (hasUser == null)
            {
                ModelState.AddModelError(String.Empty, "Kullanıcı bulunamamaıştır");
                return View();
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, model.Password);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla yenilenmiştir.";
            }

            else
            {
                ModelState.AddModelErrolList(result.Errors.Select(x => x.Description).ToList());
               
            }
            return View();

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}