using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Album.Data;
using Album.Models;
using Microsoft.AspNetCore.Authorization;

namespace Album.Areas.Admin.Pages.Blog
{
    //Admin,A Marketing Management,A Marketing Coordinator,Student
    [Authorize(Roles = "Admin,A Marketing Management,A Marketing Coordinator")]

    public class DetailsModel : PageModel
    {
        private readonly Album.Data.AppDbContext _context;

        [BindProperty]
        public List<Deadline> DeadlineList { get; set; }

        public DetailsModel(Album.Data.AppDbContext context)
        {
            _context = context;
        }

        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DeadlineList = _context.Deadline.Where(a => a.ArticleId == id).ToList();
            

            Article = await _context.Article.FirstOrDefaultAsync(m => m.ID == id);

            if (Article == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
