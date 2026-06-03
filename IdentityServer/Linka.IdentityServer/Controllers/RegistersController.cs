using Linka.IdentityServer.Dtos;
using Linka.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Linka.IdentityServer.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private const string DefaultRole =
            "Customer";

        private readonly UserManager<ApplicationUser>
            _userManager;

        private readonly RoleManager<IdentityRole>
            _roleManager;

        public RegistersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager =
                userManager;

            _roleManager =
                roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister(
            UserRegisterDto userRegisterDto)
        {
            /*
             * Register formunu kullanan herkes yalnızca
             * Customer rolüyle oluşturulur.
             *
             * Admin ve Manager rolleri formdan alınmaz.
             */
            if (!await _roleManager
                    .RoleExistsAsync(
                        DefaultRole))
            {
                var roleCreateResult =
                    await _roleManager
                        .CreateAsync(
                            new IdentityRole(
                                DefaultRole));

                if (!roleCreateResult.Succeeded)
                {
                    return BadRequest(
                        roleCreateResult.Errors
                            .Select(x =>
                                x.Description));
                }
            }

            var user =
                new ApplicationUser
                {
                    UserName =
                        userRegisterDto.Username,

                    Email =
                        userRegisterDto.Email,

                    Name =
                        userRegisterDto.Name,

                    Surname =
                        userRegisterDto.Surname
                };

            var createResult =
                await _userManager
                    .CreateAsync(
                        user,
                        userRegisterDto.Password);

            if (!createResult.Succeeded)
            {
                return BadRequest(
                    createResult.Errors
                        .Select(x =>
                            x.Description));
            }

            var roleResult =
                await _userManager
                    .AddToRoleAsync(
                        user,
                        DefaultRole);

            if (!roleResult.Succeeded)
            {
                /*
                 * Kullanıcı oluşturulduktan sonra rol ataması
                 * başarısız olursa eksik kayıt bırakmıyoruz.
                 */
                await _userManager
                    .DeleteAsync(user);

                return BadRequest(
                    roleResult.Errors
                        .Select(x =>
                            x.Description));
            }

            return Ok(
                "User added successfully.");
        }
    }
}