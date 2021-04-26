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

namespace Album.Areas.Admin.Pages.Deadlines
{//Admin,A Marketing Management,A Marketing Coordinator
    [Authorize(Roles = "Admin,A Marketing Management,A Marketing Coordinator")]

    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Deadline Deadline { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Deadline = await _context.Deadline.FirstOrDefaultAsync(m => m.dl_Id == id);

            if (Deadline == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Deadline = await _context.Deadline.FindAsync(id);

            if (Deadline != null)
            {
                _context.Deadline.Remove(Deadline);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
