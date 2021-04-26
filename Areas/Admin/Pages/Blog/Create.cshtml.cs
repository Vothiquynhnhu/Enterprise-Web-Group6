using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Album.Data;
using Album.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Album.Areas.Admin.Pages.Blog
{//Admin,A Marketing Management,A Marketing Coordinator

    [Authorize(Roles = "Admin,A Marketing Management,A Marketing Coordinator")]
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public List<Course> CourseList { get; set; }

        public string nameCourse { get; set; }

        [BindProperty]
        public string strSelect { get; set; }

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var course = _context.Courses;

            CourseList = course.ToList();

            return Page();
        }

        [BindProperty]
        public Article Article { get; set; }

        

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var course = from a in _context.Courses select a;

            course = course.Where(course => course.course_Name.Contains(strSelect));


            foreach (var item in course.ToList())
            {
                Article.courseId = item.course_Id;
            }

             

            _context.Article.Add(Article);

            await _context.SaveChangesAsync();


            // Hien thi ra trang

            

            CourseList = course.ToList();

            return RedirectToPage("./Index");
        }


    }
}
