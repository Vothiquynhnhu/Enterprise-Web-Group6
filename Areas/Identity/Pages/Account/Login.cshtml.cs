using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Album.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using XTLASPNET;

namespace Album.Areas.Identity.Pages.Account {
   [AllowAnonymous]
    public class LoginModel : PageModel {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel (SignInManager<AppUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<AppUser> userManager) {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel {
            [Required (ErrorMessage = "Successfully logged out")]
            [Display (Name = "Enter your username or email")]
            [StringLength (100, MinimumLength = 1, ErrorMessage = "Enter the correct information")]
            public string UserNameOrEmail { set; get; }

            

            [Required]
            [DataType (DataType.Password)]
            [Display(Name = "password")]
            public string Password { get; set; }

            [Display (Name = "Remember your login information?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync (string returnUrl = null) {
            if (!string.IsNullOrEmpty (ErrorMessage)) {
                ModelState.AddModelError (string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content ("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync (IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync ()).ToList ();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync (string returnUrl = null) {
            returnUrl = returnUrl ?? Url.Content ("~/");
            // Đã đăng nhập nên chuyển hướng về Index
            if (_signInManager.IsSignedIn (User)) return Redirect ("Index");

            if (ModelState.IsValid) {
                Data.Data.UserName = Input.UserNameOrEmail;
                IdentityUser user = await _userManager.FindByEmailAsync (Input.UserNameOrEmail);
                if (user == null) 
                    user = await _userManager.FindByNameAsync(Input.UserNameOrEmail);

                if (user == null) 
                {
                    ModelState.AddModelError (string.Empty, "Account does not exist.");
                    return Page ();
                }   

                var result = await _signInManager.PasswordSignInAsync (
                        user.UserName,
                        Input.Password,
                        Input.RememberMe,
                        true
                    );
                

                if (result.Succeeded) {

                    
                    _logger.LogInformation ("User logged in");
                    return ViewComponent(MessagePage.COMPONENTNAME, new MessagePage.Message() {
                        title = "Đã đăng nhập",
                        htmlcontent = "Đăng nhập thành công",
                        urlredirect = returnUrl
                    });
                }
                if (result.RequiresTwoFactor) {
                    // Nếu cấu hình đăng nhập hai yếu tố thì chuyển hướng đến LoginWith2fa
                    return RedirectToPage ("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut) {
                    _logger.LogWarning ("Account temporarily locked.");
                    // Chuyển hướng đến trang Lockout - hiện thị thông báo
                    return RedirectToPage ("./Lockout");
                } else {
                    ModelState.AddModelError (string.Empty, "Can not login.");
                    return Page ();
                }
            }
            return Page ();
            
        }
    }
}