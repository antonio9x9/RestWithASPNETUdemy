using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository.Generic;
using RestWithASPNETUdemy.Repository.Implementation;
using System;

namespace RestWithASPNETUdemy.Repository
{
    public sealed class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(MySQLContext context) : base(context) { }

        public Person Disable(long id)
        {
            var user = _context.Persons.FirstOrDefault(u => u.Id == id);
            if (user is null)
            {
                return null;
            }

            user.Enabled = false;

            try
            {
                _context.Entry(user).CurrentValues.SetValues(user);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return user;
        }

        public List<Person> FindByName(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return _context.Persons.Where(p => p.FirstName.Contains(firstName) && p.FirstName.Contains(lastName)).ToList();
            }

            else if (string.IsNullOrEmpty(firstName))
            {
                return _context.Persons.Where(p => p.FirstName.Contains(firstName)).ToList();
            }

            else if (string.IsNullOrEmpty(lastName))
            {
                return _context.Persons.Where(p => p.FirstName.Contains(lastName)).ToList();
            }
             
            return [];
        }
    }
}
