using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicGallery.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ComicGallery.Pages.Manage
{
    [Authorize(Roles ="Admin")]
    public class UsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UsersModel(UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<ApplicationUser> Users { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _userManager.Users.ToListAsync();
            return Page();
        }
    }
}
