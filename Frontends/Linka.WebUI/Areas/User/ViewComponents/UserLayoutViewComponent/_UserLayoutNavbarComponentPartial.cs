using Linka.WebUI.Models;
using Linka.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.User.ViewComponents.UserLayoutViewComponent
{
    public class _UserLayoutNavbarComponentPartial : ViewComponent
    {
        private readonly IUserService _userService;

        public _UserLayoutNavbarComponentPartial(
            IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new UserLayoutNavbarViewModel();

            try
            {
                var user = await _userService.GetUserInfo();

                model.FirstName = user.Name;
                model.LastName = user.Surname;
                model.Email = user.Email;
            }
            catch
            {
                model.FirstName = "LINKA";
                model.LastName = "User";
            }

            return View(model);
        }
    }
}