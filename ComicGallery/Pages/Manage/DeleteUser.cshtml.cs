using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicGallery.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ComicGallery.Pages.Manage
{
    public class DeleteUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }

        [BindProperty]
        public bool Confirm { get; set; }

        public ApplicationUser UserToDelete { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.Users.Where(u => u.Id == Id).SingleOrDefaultAsync();
            if (user == null)
                return NotFound();
            UserToDelete = user;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            ApplicationUser user = await _userManager.Users.Where(u => u.Id == Id).SingleOrDefaultAsync();
            if (user == null)
                return NotFound();
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            return RedirectToPage("/Manage/Users");
        }
    }
}
