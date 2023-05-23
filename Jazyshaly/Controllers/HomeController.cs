using Jazyshaly.Areas.Identity.Data;
using Jazyshaly.Data;
using Jazyshaly.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jazyshaly.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var messages = await _context.Messages.ToListAsync();
            if (startDate >= DateTime.Now || startDate == null || endDate == null)
            {
                //ModelState.AddModelError(string.Empty, "Вы ввели не верные даты.");
				Console.WriteLine("Вы ввели не верные даты.");
			}
            else if (startDate != null && endDate != null)
            {
                messages = await _context.Messages.Where(x => x.When > startDate && x.When < endDate).ToListAsync();
                ViewBag.ToplamMessage = messages.Count();
            }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.ToplamMessage = messages.Count();
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Message message)
        {
            string returnUrl = null;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                message.SenderName = User.Identity.Name;
                var sender = await _userManager.GetUserAsync(User);
                message.UserId = sender.Id;
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                return Redirect(returnUrl);
            }
            return BadRequest("Error");
        }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel Input)
        {
            string returnUrl = null;
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var data = new User { UserName = Input.UserName, Email = Input.UserName };
                var result = await _userManager.CreateAsync(data, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _signInManager.SignInAsync(data, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel Input)
        {
            string returnUrl = null;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View("Index");
                }
            }
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage("Index", "Home");
            }
        }
    }
}
