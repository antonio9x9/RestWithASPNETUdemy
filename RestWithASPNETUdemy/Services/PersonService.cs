using RestWithASPNETUdemy.Data.Converter.Implementation;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Utils;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Repository.Implementation;
using RestWithASPNETUdemy.Services.Implementations;

namespace RestWithASPNETUdemy.Services
{
    public sealed class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        private readonly PersonConverter _converter;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
            _converter = new();
        }

        public IList<PersonVO> FindAll()
        {
            return _converter.Parse(_personRepository.FindAll().ToList());
        }

        public PersonVO FindById(int id)
        {
            return _converter.Parse(_personRepository.FindById(id));
        }

        public List<PersonVO> FindByName(string firstName, string lastName)
        {
            return _converter.Parse(_personRepository.FindByName(firstName, lastName));
        }

        public PersonVO Create(PersonVO person)
        {
            Person personEntity = _converter.Parse(person);
            personEntity = _personRepository.Create(personEntity);

            return _converter.Parse(personEntity);
        }

        public PersonVO Update(PersonVO person)
        {
            Person personEntity = _converter.Parse(person);
            personEntity = _personRepository.Update(personEntity);

            return _converter.Parse(personEntity);
        }

        public PersonVO Disable(long id)
        {
            Person user = _personRepository.Disable(id);

            return _converter.Parse(user);
        }

        public void Delete(int id)
        {
            _personRepository.Delete(id);
        }

        public PageSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrEmpty(sortDirection) && !sortDirection.Equals("desc")) ? "asc" : "desc";
            var size = pageSize < 1 ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            //for don't break the query when no generate result in where
            //where 1 = 1

            string query = @"select * from person as p where 1 = 1";

            if (!string.IsNullOrEmpty(name))
            {
                query += $" and p.first_name like '%{name}%'";
            }

            query += $" order by p.first_name {sort} limit {size} offset {offset}";

            string countQuery = @"select count(*) from person as p where 1 = 1";
            if (!string.IsNullOrEmpty(name))
            {
                countQuery += $" and p.first_name like '%{name}%'";
            }
            var persons = _personRepository.FindWithPagedSearch(query).ToList();
            int totalResults = _personRepository.GetCount(countQuery);

            return new()
            {
                CurrentPage = page,
                List = _converter.Parse(persons),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }
    }
}
