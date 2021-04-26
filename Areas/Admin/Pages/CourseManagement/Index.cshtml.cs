using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Album.Data;
using Album.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Album.Areas.Admin.Pages.CourseManagement
{//Admin,A Marketing Management,A Marketing Coordinator,Student
    [Authorize(Roles = "Admin,A Marketing Management,A Marketing Coordinator,Student")]

    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Course> Course { get; set; }

        // Chuỗi để tìm kiếm, được binding tự động kể cả là truy 
        // cập get
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public async Task OnGetAsync()
        {

            // Truy vấn lấy các Article
            var course = from a in _context.Courses select a;

            //string aa = deadline.ToString();

            if (!string.IsNullOrEmpty(SearchString))
            {
                Console.WriteLine(SearchString);
                // Truy vấn lọc các Article mà tiêu đề chứa chuỗi tìm kiếm
                course = course.Where(deadline => deadline.course_Name.Contains(SearchString));
            }
            // Đọc (nạp) Article

            Course = await course.ToListAsync();

           
        }
    }
}
