using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Repository.Implementation
{
    public interface IBookRepository
    {
        List<Book> FindByName(string firstName);
    }
}
