using IdentityServer4;
using Linka.IdentityServer.Data;
using Linka.IdentityServer.Dtos.UserRoleDtos;
using Linka.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Linka.IdentityServer.Controllers
{
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static readonly string[]
            AllowedRoles =
            {
                "Customer",
                "Manager",
                "Admin"
            };

        private readonly UserManager<ApplicationUser>
            _userManager;

        private readonly RoleManager<IdentityRole>
            _roleManager;
        private readonly ApplicationDbContext
    _context;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager =
                userManager;

            _roleManager =
                roleManager;
            _context = context;
        }

        /*
         * Giriş yapan kullanıcının kendi bilgileri.
         * WebUI cookie oluştururken roller de alınır.
         */
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var userId =
                GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(
                    "User information could not be found.");
            }

            var user =
                await _userManager
                    .FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(
                    "User could not be found.");
            }

            var roles =
                await _userManager
                    .GetRolesAsync(user);

            return Ok(new
            {
                Id =
                    user.Id,

                Name =
                    user.Name,

                Surname =
                    user.Surname,

                Email =
                    user.Email,

                Username =
                    user.UserName,

                Roles =
                    roles
            });
        }

        /*
         * Mesajlaşma ekranında karşı tarafın adını göstermek
         * için kullanılabilir. Hassas alanlar dönmez.
         */
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(
                    "User ID cannot be empty.");
            }

            var user =
                await _userManager
                    .FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(
                    "User could not be found.");
            }

            return Ok(new
            {
                Id =
                    user.Id,

                Name =
                    user.Name,

                Surname =
                    user.Surname,

                Email =
                    user.Email,

                Username =
                    user.UserName
            });
        }

        /*
         * Kullanıcı listesini Admin veya Manager görüntüleyebilir.
         * ApplicationUser nesnesinin tamamını dönmüyoruz.
         * PasswordHash ve SecurityStamp gibi alanlar dışarı çıkmaz.
         */
        [HttpGet("GetAllUserList")]
        public async Task<IActionResult>
    GetAllUserList(
        string? search,
        string? roleFilter = "All",
        int page = 1,
        int pageSize = 10)
        {
            /*
             * Kullanıcı listesini yalnızca Admin veya Manager
             * görüntüleyebilir.
             */
            if (!await CurrentUserIsAdminOrManager())
            {
                return Forbid();
            }

            if (page < 1)
            {
                page = 1;
            }

            var allowedPageSizes =
                new[]
                {
            10,
            20,
            50
                };

            if (!allowedPageSizes.Contains(
                    pageSize))
            {
                pageSize =
                    10;
            }

            var query =
                _context
                    .Users
                    .AsNoTracking()
                    .AsQueryable();

            /*
             * Search:
             * Öncelikli olarak username aranır.
             * Kullanım kolaylığı için isim, soyisim ve e-posta
             * alanları da aramaya dahil edilmiştir.
             */
            if (!string.IsNullOrWhiteSpace(
                    search))
            {
                var pattern =
                    $"%{search.Trim()}%";

                query =
                    query.Where(user =>
                        (
                            user.UserName != null &&
                            EF.Functions.Like(
                                user.UserName,
                                pattern)
                        ) ||
                        (
                            user.Email != null &&
                            EF.Functions.Like(
                                user.Email,
                                pattern)
                        ) ||
                        (
                            user.Name != null &&
                            EF.Functions.Like(
                                user.Name,
                                pattern)
                        ) ||
                        (
                            user.Surname != null &&
                            EF.Functions.Like(
                                user.Surname,
                                pattern)
                        ));
            }

            /*
             * Role filtresi:
             * All seçilirse filtre uygulanmaz.
             * Unassigned seçilirse hiçbir role sahip olmayan
             * eski kullanıcılar listelenir.
             */
            if (!string.IsNullOrWhiteSpace(
                    roleFilter) &&
                !string.Equals(
                    roleFilter,
                    "All",
                    StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(
                        roleFilter,
                        "Unassigned",
                        StringComparison.OrdinalIgnoreCase))
                {
                    query =
                        query.Where(user =>
                            !_context
                                .UserRoles
                                .Any(userRole =>
                                    userRole.UserId ==
                                    user.Id));
                }
                else
                {
                    var selectedRole =
                        AllowedRoles
                            .FirstOrDefault(role =>
                                string.Equals(
                                    role,
                                    roleFilter.Trim(),
                                    StringComparison
                                        .OrdinalIgnoreCase));

                    if (selectedRole == null)
                    {
                        return BadRequest(
                            "Invalid role filter.");
                    }

                    query =
                        query.Where(user =>
                            _context
                                .UserRoles
                                .Any(userRole =>
                                    userRole.UserId ==
                                    user.Id &&
                                    _context
                                        .Roles
                                        .Any(role =>
                                            role.Id ==
                                            userRole.RoleId &&
                                            role.Name ==
                                            selectedRole)));
                }
            }

            var totalCount =
                await query.CountAsync();

            var totalPages =
                Math.Max(
                    1,
                    (int)Math.Ceiling(
                        totalCount /
                        (double)pageSize));

            if (page > totalPages)
            {
                page =
                    totalPages;
            }

            var users =
                await query
                    .OrderBy(user =>
                        user.UserName)
                    .Skip(
                        (page - 1) *
                        pageSize)
                    .Take(
                        pageSize)
                    .ToListAsync();

            var result =
                new List<ResultUserWithRoleDto>();

            foreach (var user in users)
            {
                var roles =
                    await _userManager
                        .GetRolesAsync(user);

                result.Add(
                    new ResultUserWithRoleDto
                    {
                        Id =
                            user.Id,

                        Username =
                            user.UserName ??
                            string.Empty,

                        Email =
                            user.Email ??
                            string.Empty,

                        Name =
                            user.Name ??
                            string.Empty,

                        Surname =
                            user.Surname ??
                            string.Empty,

                        RoleName =
                            roles.FirstOrDefault() ??
                            "Unassigned"
                    });
            }

            return Ok(
                new PagedResultDto<
                    ResultUserWithRoleDto>
                {
                    Items =
                        result,

                    Page =
                        page,

                    PageSize =
                        pageSize,

                    TotalCount =
                        totalCount,

                    TotalPages =
                        totalPages,

                    Search =
                        search?.Trim() ??
                        string.Empty,

                    RoleFilter =
                        string.IsNullOrWhiteSpace(
                            roleFilter)
                            ? "All"
                            : roleFilter
                });
        }

        /*
         * Rol değiştirme işlemini yalnızca Admin yapabilir.
         * Manager kullanıcıları yönetim panelini görebilir
         * ancak başka bir kullanıcıyı Admin yapamaz.
         */
        [HttpPut("UpdateUserRole")]
        public async Task<IActionResult>
            UpdateUserRole(
                UpdateUserRoleDto dto)
        {
            if (!await CurrentUserIsAdmin())
            {
                return Forbid();
            }

            if (string.IsNullOrWhiteSpace(
                    dto.UserId))
            {
                return BadRequest(
                    "User ID cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(
                    dto.RoleName))
            {
                return BadRequest(
                    "Role name cannot be empty.");
            }

            var selectedRole =
                AllowedRoles
                    .FirstOrDefault(x =>
                        string.Equals(
                            x,
                            dto.RoleName.Trim(),
                            StringComparison
                                .OrdinalIgnoreCase));

            if (selectedRole == null)
            {
                return BadRequest(
                    "Invalid role. Allowed values: " +
                    "Customer, Manager, Admin.");
            }

            var targetUser =
                await _userManager
                    .FindByIdAsync(
                        dto.UserId);

            if (targetUser == null)
            {
                return NotFound(
                    "User could not be found.");
            }

            var currentUserId =
                GetCurrentUserId();

            /*
             * Admin yanlışlıkla kendi rolünü kaldırıp
             * yönetim panelinin dışında kalmasın.
             */
            if (targetUser.Id ==
                currentUserId)
            {
                return BadRequest(
                    "You cannot change your own role.");
            }

            if (!await _roleManager
                    .RoleExistsAsync(
                        selectedRole))
            {
                return BadRequest(
                    "Selected role does not exist.");
            }

            var currentRoles =
                await _userManager
                    .GetRolesAsync(
                        targetUser);

            /*
             * Kullanıcının eski LINKA rollerini kaldırıyoruz.
             * RoleName = NULL olan eski kullanıcılarda bu liste
             * zaten boş gelir ve hata oluşmaz.
             */
            var linkaRolesToRemove =
                currentRoles
                    .Where(x =>
                        AllowedRoles.Contains(
                            x,
                            StringComparer
                                .OrdinalIgnoreCase))
                    .ToList();

            if (linkaRolesToRemove.Any())
            {
                var removeResult =
                    await _userManager
                        .RemoveFromRolesAsync(
                            targetUser,
                            linkaRolesToRemove);

                if (!removeResult.Succeeded)
                {
                    return BadRequest(
                        removeResult.Errors
                            .Select(x =>
                                x.Description));
                }
            }

            var addResult =
                await _userManager
                    .AddToRoleAsync(
                        targetUser,
                        selectedRole);

            if (!addResult.Succeeded)
            {
                return BadRequest(
                    addResult.Errors
                        .Select(x =>
                            x.Description));
            }

            return Ok(
                "User role updated successfully.");
        }

        private string? GetCurrentUserId()
        {
            return User.Claims
                .FirstOrDefault(x =>
                    x.Type ==
                    JwtRegisteredClaimNames.Sub)
                ?.Value;
        }

        private async Task<bool>
            CurrentUserIsAdmin()
        {
            var currentUserId =
                GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(
                    currentUserId))
            {
                return false;
            }

            var currentUser =
                await _userManager
                    .FindByIdAsync(
                        currentUserId);

            if (currentUser == null)
            {
                return false;
            }

            return await _userManager
                .IsInRoleAsync(
                    currentUser,
                    "Admin");
        }

        private async Task<bool>
            CurrentUserIsAdminOrManager()
        {
            var currentUserId =
                GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(
                    currentUserId))
            {
                return false;
            }

            var currentUser =
                await _userManager
                    .FindByIdAsync(
                        currentUserId);

            if (currentUser == null)
            {
                return false;
            }

            var isAdmin =
                await _userManager
                    .IsInRoleAsync(
                        currentUser,
                        "Admin");

            var isManager =
                await _userManager
                    .IsInRoleAsync(
                        currentUser,
                        "Manager");

            return isAdmin ||
                   isManager;
        }
    }
}