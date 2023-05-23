using Jazyshaly.Data;
using Jazyshaly.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jazyshaly.ViewComponents
{
	public class UserOnline : ViewComponent
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userManager;

		public UserOnline(ApplicationDbContext context,UserManager<User> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
		public IViewComponentResult Invoke()
		{
            var allUsers = _userManager.Users;
            return View(allUsers);
		}

	}
}
