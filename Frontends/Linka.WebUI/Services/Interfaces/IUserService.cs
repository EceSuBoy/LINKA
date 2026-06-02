using Linka.WebUI.Models;

namespace Linka.WebUI.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDetailViewModel> GetUserInfo();

        Task<UserDetailViewModel> GetUserByIdAsync(string id);
    }
}
