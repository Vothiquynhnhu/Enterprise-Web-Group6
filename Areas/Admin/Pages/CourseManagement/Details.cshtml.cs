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

namespace Album.Areas.Admin.Pages.CourseManagement
{//Admin,A Marketing Management,A Marketing Coordinator,Student
    [Authorize(Roles = "Admin,A Marketing Management,A Marketing Coordinator,Student")]

    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Courses.FirstOrDefaultAsync(m => m.course_Id == id);

            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
