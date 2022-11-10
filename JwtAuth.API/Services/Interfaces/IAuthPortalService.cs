using JwtAuth.API.APIModels;

namespace JwtAuth.API.Services.Interfaces
{
    public interface IAuthPortalService
    {
        Task<AuthTokenResponse> Register(UserRequest user, string accessToken);
        Task<AuthLoginResponse> Login(UserRequest userRequest);
        Task<AutRefreshTokenResponse> RecycleRefreshToken(string userName, string refreshToken);
        Task<UserLogoutResponse> Logout(string userName, string refreshToken);
    }
}