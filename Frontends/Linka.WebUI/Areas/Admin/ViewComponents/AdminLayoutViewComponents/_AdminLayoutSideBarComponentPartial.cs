using Linka.WebUI.Models;
using Linka.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.Admin
    .ViewComponents
    .AdminLayoutViewComponents
{
    public class
        _AdminLayoutSideBarComponentPartial
        : ViewComponent
    {
        private readonly IUserService
            _userService;

        public
            _AdminLayoutSideBarComponentPartial(
                IUserService userService)
        {
            _userService =
                userService;
        }

        public async Task<IViewComponentResult>
            InvokeAsync()
        {
            var user =
                await _userService
                    .GetUserInfo();

            var model =
                new AdminLayoutUserViewModel
                {
                    User =
                        user
                };

            return View(model);
        }
    }
}