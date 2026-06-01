using Linka.WebUI.Models;
using Linka.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.DefaultViewComponents
{
    public class _UserLoginDefaultComponentPartial : ViewComponent
    {
        private readonly IUserService _userService;

        public _UserLoginDefaultComponentPartial(
            IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new HeaderUserViewModel
            {
                IsAuthenticated =
                    User.Identity?.IsAuthenticated == true,

                CanAccessAdminPanel =
                    User.IsInRole("Admin") ||
                    User.IsInRole("Manager")
            };

            if (!model.IsAuthenticated)
            {
                return View(model);
            }

            try
            {
                var user = await _userService.GetUserInfo();

                model.FullName =
                    $"{user.Name} {user.Surname}".Trim();
            }
            catch
            {
                model.FullName =
                    User.Identity?.Name ?? "My Account";
            }

            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                model.FullName = "My Account";
            }

            return View(model);
        }
    }
}