using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Repository.Implementation
{
    public interface IUserRepository
    {
        User RefreshUserInfo(UserVO user);
        User ValidateCredentials(UserVO user);
        User ValidateCredentials(string userName);

        bool RevokeToken(string username);
    }
}
