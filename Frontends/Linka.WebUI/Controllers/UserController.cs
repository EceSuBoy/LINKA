using Linka.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userservice;

        public UserController(IUserService userservice)
        {
            _userservice = userservice;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _userservice.GetUserInfo();  
            return View();
        }
    }
}
