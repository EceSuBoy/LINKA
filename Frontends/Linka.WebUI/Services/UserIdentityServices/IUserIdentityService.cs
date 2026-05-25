using Linka.DtoLayer.IdentityDtos.UserDtos;

namespace Linka.WebUI.Services.UserIdentityServices
{
    public interface IUserIdentityService
    {
        Task<List<ResultUserDto>> GetAllUserListAsync();
    }
}
