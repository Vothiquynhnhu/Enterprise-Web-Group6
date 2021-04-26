using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Album.Areas.Admin.Pages.fileManagement
{
    public class OpenFilePdfModel : PageModel
    {
        [BindProperty]
        public string NameFile { get; set; }
        public string filePath { get; set; }
        public string filePath2 { get; set; }

        private IHostingEnvironment _environment;

        public OpenFilePdfModel(IHostingEnvironment environment)
        {
           _environment = environment;
        }
        public void OnGet(string fileName)
        {

            NameFile = "aaa-a.pdf";
        }


    }
}
