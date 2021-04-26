using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Album.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Album.Models;
using Microsoft.AspNetCore.Authorization;

namespace Album.Areas.Admin.Pages.CourseManagement
{//Admin,A Marketing Management,A Marketing Coordinator
    [Authorize(Roles = "Admin,A Marketing Management,A Marketing Coordinator")]

    public class AddUserModel : PageModel
    {

        private readonly AppDbContext _context;

        [BindProperty]
        public List<AppUser> AppUserList { get; set; }

        [BindProperty]
        public List<Course> CourseList { get; set; }

        [BindProperty]
        public RegisterCourse RegisterCourse { get; set; }

        [BindProperty]
        public string NameUser { get; set; }
        [BindProperty]
        public string NameCourse { get; set; } 

        public AddUserModel(AppDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {

            AppUserList = _context.Users.ToList();

            CourseList = _context.Courses.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Name User to Id
            List<AppUser> idUser = _context.Users.Where(a => a.FullName.Contains(NameUser)).ToList();


            // Name Course to Id 

            List<Course> idCourse = _context.Courses.Where(a => a.course_Name.Contains(NameCourse)).ToList();

            // Add to RegisterCourse

            RegisterCourse = new RegisterCourse()
            {
                CourseId = idCourse[0].course_Id,
                UserId = idUser[0].Id
            };

            _context.RegisterCourse.Add(RegisterCourse);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
