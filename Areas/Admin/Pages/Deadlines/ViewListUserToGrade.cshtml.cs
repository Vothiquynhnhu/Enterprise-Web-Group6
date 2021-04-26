using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Album.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Album.Models;
using Microsoft.EntityFrameworkCore;

namespace Album.Areas.Admin.Pages.Deadlines
{
    public class ViewListUserToGradeModel : PageModel
    {
        private readonly AppDbContext _context;
        
        [BindProperty]
        public GradeManagementt gradeManagemet { get; set; }

        [BindProperty]
        public GradeManagementt gradeManagemetDelete { get; set; }

        [BindProperty]
        public List<ViewUserGrade> viewUserGradesList { get; set; }

        public ViewListUserToGradeModel(AppDbContext context)
        {
            _context = context;
        }
        public async Task OnGetAsync()
        {
            var query = from a in _context.Users
                       join b in _context.RegisterCourse on a.Id equals b.UserId
                       join c in _context.Courses on b.CourseId equals c.course_Id
                       join d in _context.Article on c.course_Id equals d.courseId
                       join e in _context.Deadline on d.ID equals e.ArticleId
                       select new { a, b, c, d, e, };

            //var query1 = from k in _context.gradeManagemens
                        // join f in _context.Deadline on k.DeadIdG equals f.dl_Id
                       //  select new { k,f};



            var data = await query
               .Select(x => new ViewUserGrade()
               {
                   FullName = x.a.FullName,
                   NameEvent = x.d.Title,
                   NameDeadline =x.e.Title,
                   DeadlineId = x.e.dl_Id
               }).ToListAsync();

            viewUserGradesList = data.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            foreach (var item in viewUserGradesList)
            {
                if(item.NumberGrade != 0)
                {
                   // var dataupdate = _context.gradeManagemens.Where(a => a.DeadIdG == item.DeadlineId);
                    var userGradeQuery = from aa in _context.gradeManagemens select aa;
                   // if (dataupdate.Count() != 0)
                   //{
                        //for (int i = 0; i < viewUserGradesList.Count; i++)
                        //{
                            //var buserFile = userGradeQuery.Where(a => a.DeadIdG == item.DeadlineId);

                            //if (buserFile == null)
                            //{
                            //    return BadRequest("false");
                            //}
                      //  var fileGradeId = from a in _context.gradeManagemens where a.DeadIdG == item.DeadlineId select a.Id;
                        //foreach (var item44 in fileGradeId)
                        //{
                        //    var files = await _context.gradeManagemens.FindAsync(item44);
                        //    int aaa = item.NumberGrade;
                        //    files.grade = aaa;
                            
                        //}
                        //_context.Update(files);
                        //await _context.SaveChangesAsync();



                    //}


                    gradeManagemet.grade = item.NumberGrade;
                     //   gradeManagemet.DeadIdG = item.DeadlineId;

                        _context.gradeManagemens.Update(gradeManagemet);
                        await _context.SaveChangesAsync();
                }
            }

            



            // Print
            var query = from a in _context.Users
                        join b in _context.RegisterCourse on a.Id equals b.UserId
                        join c in _context.Courses on b.CourseId equals c.course_Id
                        join d in _context.Article on c.course_Id equals d.courseId
                        join e in _context.Deadline on d.ID equals e.ArticleId
                        select new { a, b, c, d, e, };

            var query1 = from k in _context.gradeManagemens
                      //   join f in _context.Deadline on k.DeadIdG equals f.dl_Id
                         select new { k };



            var data = await query
               .Select(x => new ViewUserGrade()
               {
                   FullName = x.a.FullName,
                   NameEvent = x.d.Title,
                   NameDeadline = x.e.Title,
                   DeadlineId = x.e.dl_Id
               }).ToListAsync();

            viewUserGradesList = data.ToList();

            return Page();
        }
    }
}
