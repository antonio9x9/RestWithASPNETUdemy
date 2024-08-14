using RestWithASPNETUdemy.Data.VO;

namespace RestWithASPNETUdemy.Services.Implementations
{
    public interface ILoginService
    {
        TokenVO ValidateCredentials(UserVO user);
        TokenVO ValidateCredentials(TokenVO token);
        bool RevokeToken(string username);
    }
}
