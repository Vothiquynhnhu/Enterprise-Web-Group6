using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Album.Data;
using Album.Models;
using Microsoft.AspNetCore.Authorization;

namespace Album.Areas.Admin.Pages.Blog
{//Admin,A Marketing Management,A Marketing Coordinator,Student
    [Authorize(Roles = "Admin,A Marketing Management,A Marketing Coordinator,Student")]

    public class ViewFindId : PageModel
    {
        private readonly AppDbContext _context;

        public ViewFindId(AppDbContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; }
        

        // Chuỗi để tìm kiếm, được binding tự động kể cả là truy 
        // cập get
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            // Truy vấn lấy các Article
            var articles = from a in _context.Article select a;



            if (id != null)
            {
                articles = articles.Where(article => article.ID == id);
            }

            // Đọc (nạp) Article
            Article = await articles.ToListAsync();

            return Page();
        }


    }
}
