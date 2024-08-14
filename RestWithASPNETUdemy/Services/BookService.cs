using RestWithASPNETUdemy.Data.Converter.Implementation;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Utils;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Repository;
using RestWithASPNETUdemy.Repository.Implementation;
using RestWithASPNETUdemy.Services.Implementations;
using System;

namespace RestWithASPNETUdemy.Services
{
    public sealed class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;

        private readonly BookConverter _converter;

        public BookService(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
            _converter = new();
        }

        public BookVO Create(BookVO book)
        {
            Book personEntity = _converter.Parse(book);
            personEntity = _bookRepository.Create(personEntity);

            return _converter.Parse(personEntity);
        }

        public void Delete(int id)
        {
            _bookRepository.Delete(id);
        }

        public IList<BookVO> FindAll()
        {
            return _converter.Parse(_bookRepository.FindAll().ToList());
        }

        public BookVO FindById(int id)
        {
            return _converter.Parse(_bookRepository.FindById(id));
        }

        public BookVO Update(BookVO book)
        {
            Book personEntity = _converter.Parse(book);
            personEntity = _bookRepository.Update(personEntity);

            return _converter.Parse(personEntity);
        }

        public PageSearchVO<BookVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrEmpty(sortDirection) && !sortDirection.Equals("desc")) ? "asc" : "desc";
            var size = pageSize < 1 ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;
             
            //for don't break the query when no generate result in where
            //where 1 = 1

            string query = @"select * from books as p where 1 = 1";

            if (!string.IsNullOrEmpty(name))
            {
                query += $" and p.title like '%{name}%'";
            }

            query += $" order by p.title {sort} limit {size} offset {offset}";

            string countQuery = @"select count(*) from books as p where 1 = 1";
            if (!string.IsNullOrEmpty(name))
            {
                countQuery += $" and p.title like '%{name}%'";
            }
            var persons = _bookRepository.FindWithPagedSearch(query).ToList();
            int totalResults = _bookRepository.GetCount(countQuery);

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
