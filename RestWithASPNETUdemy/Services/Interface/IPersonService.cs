using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Utils;
using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Services.Implementations
{
    public interface IPersonService
    {
        PersonVO Create(PersonVO person);
        PersonVO FindById(int id);
        IList<PersonVO> FindAll();
        PersonVO Update(PersonVO person);
        void Delete(int id);
        PersonVO Disable(long id);
        List<PersonVO> FindByName(string firstName, string lastName);

        PageSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
    }
}
