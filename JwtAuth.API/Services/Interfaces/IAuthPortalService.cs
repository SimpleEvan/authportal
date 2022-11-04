using JwtAuth.API.APIModels;

namespace JwtAuth.API.Services.Interfaces
{
    public interface IAuthPortalService
    {
        Task<AuthTokenResponse> Register(UserRequest user);
        Task<string> Login(UserRequest userRequest);
        UserLoggedResponse Logout(UserRequest userRequest);
    }
}