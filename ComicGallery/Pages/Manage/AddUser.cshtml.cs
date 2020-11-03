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
using Microsoft.Extensions.Options;

namespace ComicGallery.Pages.Manage
{
    [Authorize(Roles ="Admin")]
    public class AddUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IOptions<ApplicationSettings> _option;

        public AddUserModel(
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager,
            IOptions<ApplicationSettings> option
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _option = option;
        }
        
        public class InputModel
        {
            [Required]
            [Display(Name ="用户名")]
            public string UserName { get; set; }
            [Required]
            [Display(Name ="角色")]
            public string RoleName { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList Roles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
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
            ApplicationUser user = new ApplicationUser
            {
                UserName = Input.UserName
            };
            var createResult = await _userManager.CreateAsync(user, _option.Value.DefaultPassword);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
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
