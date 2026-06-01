using Linka.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserInfo();

            ViewBag.FullName = $"{user.Name} {user.Surname}";
            ViewBag.Email = user.Email;

            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}