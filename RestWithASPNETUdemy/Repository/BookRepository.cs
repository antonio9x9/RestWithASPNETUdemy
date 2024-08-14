using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository.Generic;
using RestWithASPNETUdemy.Repository.Implementation;
using System;

namespace RestWithASPNETUdemy.Repository
{
    public sealed class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(MySQLContext context) : base(context) { }

        public List<Book> FindByName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                return _context.Books.Where(p => p.Title.Contains(firstName)).ToList();
            }

            return [];
        }
    }
}
