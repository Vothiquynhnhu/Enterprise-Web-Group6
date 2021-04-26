using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Album.Data;
using Album.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Album.Services;
using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;

namespace Album.Areas.Admin.Pages.fileManagement
{//Admin,A Marketing Management,A Marketing Coordinator
    [Authorize(Roles = "Admin,A Marketing Management,A Marketing Coordinator")]

    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public UserFile userFileList { get; set; }

        private IHostingEnvironment _environment;

        [BindProperty]
        public List<FileManagement> FileManagementList { get; set; }

        [BindProperty]
        public bool checkboxaa { get; set; }

        [BindProperty]
        public bool checkboxbb { get; set; }

        [BindProperty]
        public bool down { get; set; }

        public UserFile userFile { get; set; }

        [BindProperty]
        public bool sort { get; set; }

        [BindProperty]
        public string NameSort { get; set; }

        [BindProperty]
        public List<string> sortEvent { get; set; }


        private readonly IFileService _fileService;

        public IndexModel(AppDbContext context, IFileService fileService, IHostingEnvironment environment)
        {
            _context = context;
            _fileService = fileService;
            _environment = environment;
        }
        public async Task<IActionResult> OnGet()
        {

            var dataEvent = from a in _context.Article select a.Title;

            sortEvent = await dataEvent.ToListAsync();

            //Print
            if (NameSort == "All")
            {
                var query = from a in _context.Article
                            join b in _context.Deadline on a.ID equals b.ArticleId
                            join cc in _context.userFiles on b.dl_Id equals cc.file_DeadlineId
                            select new { a, b, cc };

                var data = await query
                   .Select(x => new FileManagement()
                   {
                       NameEvent = x.a.Title,
                       NameFile = x.cc.Title,
                       NameDeadline = x.b.Title,
                       FileSelect = x.cc.file_IsSelected,
                       FileId = x.cc.ID
                   }).ToListAsync();
                var buserFileSort = data.Where(a => a.FileSelect == true);

                FileManagementList = buserFileSort.ToList();
            }
            else
            {
                var query = from a in _context.Article
                            join b in _context.Deadline on a.ID equals b.ArticleId
                            join cc in _context.userFiles on b.dl_Id equals cc.file_DeadlineId
                            select new { a, b, cc };

                var data = await query
                   .Select(x => new FileManagement()
                   {
                       NameEvent = x.a.Title,
                       NameFile = x.cc.Title,
                       NameDeadline = x.b.Title,
                       FileSelect = x.cc.file_IsSelected,
                       FileId = x.cc.ID
                   }).ToListAsync();
                var buserFileSort = data.Where(a => a.FileSelect == true && a.NameEvent == NameSort);

                FileManagementList = data.ToList();
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var userFileQuery = from aa in _context.userFiles select aa;

            for (int i = 0; i < FileManagementList.Count; i++)
            {
                var fileNameTraVe = FileManagementList[i].FileId;
                var buserFile = userFileQuery.Where(a => a.Title.Contains(FileManagementList[i].FileId.ToString()));

                if (buserFile == null)
                {
                    return BadRequest("false");
                }
                var files = await _context.userFiles.FindAsync(fileNameTraVe);
                files.ID = fileNameTraVe;
                bool aaa = FileManagementList[i].FileSelect;
                files.file_IsSelected = aaa;
                _context.Update(files);
            }
            await _context.SaveChangesAsync();

            

            if (down)
            {
                await DownloadFilesAsync();
            }

            var dataEvent = from a in _context.Article select a.Title;
            sortEvent = await dataEvent.ToListAsync();

            //Print
            if (NameSort == "All")
            {
                var query = from a in _context.Article
                            join b in _context.Deadline on a.ID equals b.ArticleId
                            join cc in _context.userFiles on b.dl_Id equals cc.file_DeadlineId
                            select new { a, b, cc };

                var data = await query
                   .Select(x => new FileManagement()
                   {
                       NameEvent = x.a.Title,
                       NameFile = x.cc.Title,
                       NameDeadline = x.b.Title,
                       FileSelect = x.cc.file_IsSelected,
                       FileId = x.cc.ID
                   }).ToListAsync();
                var buserFileSort = data.Where(a => a.FileSelect == true );

                FileManagementList = buserFileSort.ToList();
            }
            else
            {
                var query = from a in _context.Article
                            join b in _context.Deadline on a.ID equals b.ArticleId
                            join cc in _context.userFiles on b.dl_Id equals cc.file_DeadlineId
                            select new { a, b, cc };

                var data = await query
                   .Select(x => new FileManagement()
                   {
                       NameEvent = x.a.Title,
                       NameFile = x.cc.Title,
                       NameDeadline = x.b.Title,
                       FileSelect = x.cc.file_IsSelected,
                       FileId = x.cc.ID
                   }).ToListAsync();
                var buserFileSort = data.Where(a => a.FileSelect == true && a.NameEvent == NameSort);

                FileManagementList = data.ToList();
            }
            return Page();
        }



        public async Task DownloadFilesAsync(string subDirectory = "aaa")
        {
            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

            var files = Directory.GetFiles(Path.Combine(_environment.ContentRootPath, subDirectory)).ToList();

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {

                    foreach (var botFilePath in files)
                    {
                        var botFileName = Path.GetFileName(botFilePath);
                        var entry = archive.CreateEntry(botFileName);
                        using (var entryStream = entry.Open())
                        using (var fileStream = System.IO.File.OpenRead(botFilePath))
                        {
                            await fileStream.CopyToAsync(entryStream);
                        }

                    }

                }

            }

        }
    }
}
