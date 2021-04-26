using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Album.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Album.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetPasswordModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} is from {2} to {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Enter the password")]
            [Compare("Password", ErrorMessage = "Password must be the same.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("Token code is not available.");
            }
            else
            {
                Input = new InputModel
                {
                    // Giải mã lại code từ code trong url (do mã này khi gửi mail 
                    // đã thực hiện Encode bằng WebEncoders.Base64UrlEncode)
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Tìm User theo email
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Không thấy user
                return RedirectToPage("./ResetPasswordConfirmation");
            }
            // Đặt lại passowrd chu user - có kiểm tra mã token khi đổi
            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            
            if (result.Succeeded)
            {
                // Chuyển đến trang thông báo đã reset thành công
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
