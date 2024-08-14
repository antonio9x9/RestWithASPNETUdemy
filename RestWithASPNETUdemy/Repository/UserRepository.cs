using Microsoft.EntityFrameworkCore;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository.Implementation;
using System.Security.Cryptography;
using System.Text;

namespace RestWithASPNETUdemy.Repository
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly MySQLContext _context;

        public UserRepository(MySQLContext context)
        {
            _context = context;
        }

        public User ValidateCredentials(UserVO user)
        {
            var pass = ComputerHash(user.Password, SHA256.Create());

            return _context.Users.FirstOrDefault(u => u.UserName == user.UserName && u.Password == pass);
        }

        private object ComputerHash(string input, HashAlgorithm hashAlgorithm)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = hashAlgorithm.ComputeHash(inputBytes);

            var builder = new StringBuilder();

            foreach (byte item in hashedBytes)
            {
                builder.Append(item.ToString("x2"));
            }

            return builder.ToString();
        }

        public User RefreshUserInfo(UserVO user)
        {
            if (!_context.Users.Any(p => p.Id.Equals(user.Id)))
            {
                return null;
            }

            User result = _context.Users.FirstOrDefault(u => u.Id == user.Id);

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return result;
        }

        public User ValidateCredentials(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public bool RevokeToken(string username)
        {
            User result = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (result == null)
            {
                return false;
            }

            result.RefreshToken = "";
            _context.SaveChanges();

            return true;
        }
    }
}
