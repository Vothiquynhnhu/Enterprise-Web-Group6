using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Album.Data;
using Album.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Album.Areas.Admin.Pages.Deadlines
{//Admin,A Marketing Management,A Marketing Coordinator
    [Authorize(Roles = "Admin,A Marketing Management,A Marketing Coordinator")]

    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public List<Article> ArticleList { get; set; }

        [BindProperty]
        public string strSelectt { get; set; }

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var course = _context.Article;

            ArticleList = course.ToList();

            return Page();
        }

        [BindProperty]
        public Deadline Deadline { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            var article = from a in _context.Article select a;

            article = article.Where(q => q.Title.Contains(strSelectt));


            foreach (var item in article.ToList())
            {
                Deadline.ArticleId = item.ID;
            }

            _context.Deadline.Add(Deadline);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
