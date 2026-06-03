using Linka.DtoLayer.IdentityDtos.UserDtos;
using Linka.WebUI.Services.CargoServices
    .CargoCustomerServices;
using Linka.WebUI.Services.Interfaces;
using Linka.WebUI.Services.UserIdentityServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/User")]
    public class UserController
        : AdminControllerBase
    {
        private readonly IUserIdentityService
            _userIdentityService;

        private readonly ICargoCustomerService
            _cargoCustomerService;

        private readonly IUserService
            _userService;

        public UserController(
            IUserIdentityService userIdentityService,
            ICargoCustomerService cargoCustomerService,
            IUserService userService)
        {
            _userIdentityService =
                userIdentityService;

            _cargoCustomerService =
                cargoCustomerService;

            _userService =
                userService;
        }

        [HttpGet]
        [Route("UserList")]
        public async Task<IActionResult>
            UserList(
                string? search,
                string? roleFilter = "All",
                int page = 1,
                int pageSize = 10)
        {
            ViewBag.v1 =
                "Home";

            ViewBag.v2 =
                "Users";

            ViewBag.v3 =
                "User List";

            ViewBag.v0 =
                "User Role Management";

            var currentUser =
                await _userService
                    .GetUserInfo();

            ViewBag.CurrentUserId =
                currentUser.Id;

            /*
             * Manager kullanıcı listesini görebilir
             * ancak rol değiştiremez.
             */
            ViewBag.CanManageRoles =
                User.IsInRole(
                    "Admin");

            var values =
                await _userIdentityService
                    .GetAllUserListAsync(
                        search,
                        roleFilter,
                        page,
                        pageSize);

            return View(values);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("UpdateRole")]
        public async Task<IActionResult>
            UpdateRole(
                UpdateUserRoleDto dto,
                string? search,
                string? roleFilter,
                int page = 1,
                int pageSize = 10)
        {
            /*
             * UI katmanında da kontrol ediyoruz.
             * API tarafındaki güvenlik kontrolü ayrıca korunuyor.
             */
            if (!User.IsInRole(
                    "Admin"))
            {
                return Forbid();
            }

            try
            {
                await _userIdentityService
                    .UpdateUserRoleAsync(dto);

                TempData["UserRoleSuccess"] =
                    "User role updated successfully.";
            }
            catch (Exception exception)
            {
                TempData["UserRoleError"] =
                    exception.Message;
            }

            return RedirectToAction(
                nameof(UserList),
                new
                {
                    search,
                    roleFilter,
                    page,
                    pageSize
                });
        }

        [HttpGet]
        [Route("UserAddressInfo/{id}")]
        public async Task<IActionResult>
            UserAddressInfo(
                string id)
        {
            var values =
                await _cargoCustomerService
                    .GetByIdCargoCustomerInfoAsync(
                        id);

            return View(values);
        }
    }
}