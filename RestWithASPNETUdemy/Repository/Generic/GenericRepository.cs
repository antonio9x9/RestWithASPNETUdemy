using Microsoft.EntityFrameworkCore;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Base;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository.Implementation;
using System;

namespace RestWithASPNETUdemy.Repository.Generic
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected MySQLContext _context;

        private DbSet<T> _dbSet;

        public GenericRepository(MySQLContext context)
        {
            _context = context;

            //dynamic get repository type to context
            _dbSet = _context.Set<T>();
        }

        public T Create(T item)
        {
            try
            {
                _dbSet.Add(item);
                _context.SaveChanges();

                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(int id)
        {
            T result = FindById(id);

            if (result != null)
            {
                try
                {
                    _dbSet.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public IList<T> FindAll()
        {
            try
            {
                return _dbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T FindById(int id)
        {
            try
            {
                return _dbSet.SingleOrDefault(p => p.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<T> FindWithPagedSearch(string query)
        {
            return _dbSet.FromSqlRaw(query).ToList();
        }

        public int GetCount(string query)
        {
            var result = "";

            //return a count of total from the executed string
            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                
                using(var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    result = command.ExecuteScalar().ToString();
                }
            }

            return int.Parse(result);
        }

        public T Update(T item)
        {
            if (!Exists(item.Id))
            {
                return null;
            }

            T result = FindById(item.Id);

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }

                return item;
            }
            else
            {
                return null;
            }
        }

        private bool Exists(int id)
        {
            return _dbSet.Any(p => p.Id.Equals(id));
        }
    }
}
