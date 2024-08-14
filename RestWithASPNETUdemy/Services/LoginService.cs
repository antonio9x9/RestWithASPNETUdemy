using RestWithASPNETUdemy.Configurations;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Repository.Implementation;
using RestWithASPNETUdemy.Services.Implementations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestWithASPNETUdemy.Services
{
    public sealed class LoginService : ILoginService
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfiguration _configuration;

        private IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginService(TokenConfiguration configuration, IUserRepository userRepository, ITokenService tokenService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public TokenVO ValidateCredentials(UserVO userCredentials)
        {
            var user = _userRepository.ValidateCredentials(userCredentials);
            if (user == null)
            {
                return null;
            }
            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Jti, new Guid().ToString("N")),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpiry);

            _userRepository.RefreshUserInfo(userCredentials);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenVO(true,
                               createDate.ToString(DATE_FORMAT),
                               expirationDate.ToString(DATE_FORMAT),
                               accessToken,
                               refreshToken);
        }

        public TokenVO ValidateCredentials(TokenVO token)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var userName = principal.Identity.Name;

            var user = _userRepository.ValidateCredentials(userName);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return null;
            }
            accessToken = _tokenService.GenerateAccessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            _userRepository.RefreshUserInfo(new() { Id = user.Id });

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenVO(true,
                               createDate.ToString(DATE_FORMAT),
                               expirationDate.ToString(DATE_FORMAT),
                               accessToken,
                               refreshToken);
        }

        public bool RevokeToken(string username)
        {
            return _userRepository.RevokeToken(username); 
        }
    }
}
