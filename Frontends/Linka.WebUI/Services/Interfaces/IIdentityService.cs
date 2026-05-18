using Linka.DtoLayer.IdentityDtos.LoginDtos;

namespace Linka.WebUI.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> SignIn(SignInDto signInDto);
    }
}
