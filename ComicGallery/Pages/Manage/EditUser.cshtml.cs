using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ComicGallery.Common;
using ComicGallery.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace ComicGallery.Pages.Manage
{
    public class EditUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public EditUserModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public class InputModel
        {
            [Display(Name = "用户名")]
            [Editable(false)]
            public string UserName { get; set; }
            [Required]
            [Display(Name = "角色")]
            public string RoleName { get; set; }
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public SelectList Roles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.Users.Where(u => u.Id == Id).SingleOrDefaultAsync();
            if (user == null)
                return NotFound();
            Input.UserName = user.UserName;
            Input.RoleName = (await _userManager.GetRolesAsync(user)).SingleOrDefault();
            var roles = await _roleManager.Roles.ToListAsync();
            Roles = new SelectList(roles);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            ApplicationUser user = await _userManager.Users.Where(u => u.Id == Id).SingleOrDefaultAsync();

            var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            if (!removeRoleResult.Succeeded)
            {
                foreach (var error in removeRoleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            var roleResult = await _userManager.AddToRoleAsync(user, Input.RoleName);
            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            return RedirectToPage("/Manage/Users");
        }
    }
}
