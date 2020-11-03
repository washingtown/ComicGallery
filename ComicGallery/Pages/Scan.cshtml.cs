using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ComicGallery.Pages
{
    [Authorize(Roles ="Admin")]
    public class ScanModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
