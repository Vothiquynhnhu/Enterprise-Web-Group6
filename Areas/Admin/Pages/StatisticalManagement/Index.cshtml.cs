using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Album.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Album.Models;
using Microsoft.EntityFrameworkCore;

namespace Album.Areas.Admin.Pages.StatisticalManagement
{
    public class IndexModel : PageModel
    {
        private AppDbContext _Context;

        public List<UserFile> userFilesList { get; set; }

        public List<Article> articlesList { get; set; }
        
        [BindProperty]
        public List<StatisticalAsm1> statisticalAsm1List { get; set; }
        [BindProperty]
        public List<StatisticalAsm2> statisticalAsm2List { get; set; }


        public IndexModel(AppDbContext Context)
        {
            _Context = Context;
        }

        public async Task<IActionResult> OnGet()
        
        {
            var query = from a in _Context.Article
                        join b in _Context.Deadline on a.ID equals b.ArticleId
                        join c in _Context.userFiles on b.dl_Id equals c.file_DeadlineId
                        select new{a, b,c};

            //asm 1
            //var dataAsm2 = query.Where(a => a.b.Title == "Assignment2").ToList();

            var dataAsm1 = query.Where(a => a.b.Title == "Assignment1").ToList();

            var dataEvent1 = dataAsm1.Select(a => a.a.Title).Distinct().ToList();


            string NameEvent1;
            int CountDeadline1;

            statisticalAsm1List = new List<StatisticalAsm1>();

            foreach (var item1 in dataEvent1)
            {
                NameEvent1 = item1;
                CountDeadline1 = 0;
                foreach (var item2 in dataAsm1)
                {
                    if(NameEvent1 == item2.a.Title) { CountDeadline1++; }
                }
                statisticalAsm1List.Add(new StatisticalAsm1() {
                    st_NameEvent1 = NameEvent1,
                    st_Deadline1 = CountDeadline1
                });
            }



            ////asm 2
            //var dataAsm2 = query.Where(a => a.b.Title == "Assignment2").ToList();

            //var dataEvent2 = dataAsm2.Select(a => a.a.Title).Distinct().ToList();
            //string NameEvent2;
            //int CountDeadline2;
            //foreach (var item2 in dataEvent2)
            //{
            //    NameEvent2 = item2;
            //    CountDeadline2 = 0;
            //    foreach (var item3 in dataAsm2)
            //    {
            //        if (NameEvent2 == item3.a.Title) { CountDeadline2++; }
            //    }
            //    statisticalAsm2List.Add(new StatisticalAsm2()
            //    {
            //        st_NameEvent2 = NameEvent2,
            //        st_Deadline2 = CountDeadline2
            //    });
            //}
            var dataAsm2 = query.Where(a => a.b.Title == "Assignment2").ToList();

            var dataEvent2 = dataAsm2.Select(a => a.a.Title).Distinct().ToList();


            string NameEvent2;
            int CountDeadline2;

            statisticalAsm2List = new List<StatisticalAsm2>();

            foreach (var item3 in dataEvent2)
            {
                NameEvent2 = item3;
                CountDeadline2 = 0;
                foreach (var item4 in dataAsm2)
                {
                    if (NameEvent2 == item4.a.Title) { CountDeadline2++; }
                }
                statisticalAsm2List.Add(new StatisticalAsm2()
                {
                    st_NameEvent2 = NameEvent2,
                    st_Deadline2 = CountDeadline2
                });
            }

            return Page();
        }
    }
}
