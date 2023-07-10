using AspNetCoreIdentityApp.web.Areas.Admin.Models;
using AspNetCoreIdentityApp.web.Extensions;
using AspNetCoreIdentityApp.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityApp.web.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "admin,role-action")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name!
            }).ToListAsync();
            return View(roles);
        }

        [HttpGet]
        [Authorize(Roles = "role-action")]
        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "role-action")]
        public async Task<IActionResult> RoleCreate(RoleCreateViewModel request)
        {
            var result = await _roleManager.CreateAsync(new AppRole() { Name = request.Name });

            if (!result.Succeeded)
            {
                ModelState.AddModelErrolList(result.Errors);
                return View();
            }

            TempData["SuccessMessage"] = "Rol oluşturulmuştur.";

            return RedirectToAction(nameof(RolesController.Index));
        }

        [HttpGet]
        [Authorize(Roles = "role-action")]
        public async Task<IActionResult> RoleUpdate(string id)
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(id);

            if (roleToUpdate == null)
            {
                throw new Exception("Güncellenicek rol bulunamamıştır");
            }

            return View(new RoleUpdateViewModel() { Id=roleToUpdate.Id, Name=roleToUpdate!.Name!});
        }

        [HttpPost]
        [Authorize(Roles = "role-action")]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel request)
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(request.Id);

            if (roleToUpdate == null)
            {
                throw new Exception("Güncellenicek rol bulunamamıştır");
            }

            roleToUpdate.Name = request.Name;
           
            await _roleManager.UpdateAsync(roleToUpdate);

            ViewData["SuccessMessage"] = "Rol bilgisi güncellenmiştir";

            return View();
        }

        [Authorize(Roles = "role-action")]
        public async Task<IActionResult> RoleDelete(string id)
        {
            var roleToDelete = await _roleManager.FindByIdAsync(id);
            if (roleToDelete == null)
            {
                throw new Exception("Silimecek rol bulunamamıştır");
            }

            var result = await _roleManager.DeleteAsync(roleToDelete);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.Select(x=>x.Description).First());
            }
            TempData["SuccessMessage"] = "Rol Silinmiştir.";
            return RedirectToAction(nameof(RolesController.Index));

        }

        public async Task<IActionResult> AssingRoleToUser(string id)
        {
            var currentUser =(await _userManager.FindByIdAsync(id))!;
            ViewBag.userId = id;

            var roles = await _roleManager.Roles.ToListAsync();

            var roleViewModelList = new List<AssingRoleToUserViewModel>();

            var userRoles = await _userManager.GetRolesAsync(currentUser);

            foreach (var role in roles) 
            {
                var assingRoleToUserViewModel = new AssingRoleToUserViewModel() { Id = role.Id, Name = role.Name! };

                if (userRoles.Contains(role.Name!))
                {
                    assingRoleToUserViewModel.Exist = true;
                }

                roleViewModelList.Add(assingRoleToUserViewModel);
            }

            return View(roleViewModelList);

        }

        [HttpPost]
        public async Task<IActionResult> AssingRoleToUser(string userId,List<AssingRoleToUserViewModel> requestList)
        {
            var userToAssignRoles = (await _userManager.FindByIdAsync(userId))!;

            foreach (var role in requestList)
            {
                if (role.Exist)
                {
                    await _userManager.AddToRoleAsync(userToAssignRoles, role.Name);
                }
                else 
                {
                    await _userManager.RemoveFromRoleAsync(userToAssignRoles,role.Name);                
                }
            }

            return RedirectToAction(nameof(HomeController.UserList),"Home");
        }
    }
}
