using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Utils;
using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Services.Implementations
{
    public interface IBookService
    {
        BookVO Create(BookVO person);
        BookVO FindById(int id);
        IList<BookVO> FindAll();
        BookVO Update(BookVO person);
        void Delete(int id);
        PageSearchVO<BookVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
    }
}
