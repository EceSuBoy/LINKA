using Linka.DtoLayer.IdentityDtos.UserDtos;

namespace Linka.WebUI.Services
    .UserIdentityServices
{
    public interface IUserIdentityService
    {
        Task<
            PagedResultDto<
                ResultUserWithRoleDto>>
            GetAllUserListAsync(
                string? search,
                string? roleFilter,
                int page,
                int pageSize);

        Task UpdateUserRoleAsync(
            UpdateUserRoleDto dto);
    }
}