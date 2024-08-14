using RestWithASPNETUdemy.Model.Base;

namespace RestWithASPNETUdemy.Repository.Implementation
{
    public interface IRepository<T> where T : BaseEntity
    {
        public T Create(T item);

        public T Update(T item);

        public void Delete(int id);

        public T FindById(int id);

        public IList<T> FindAll();

        public IList<T> FindWithPagedSearch(string query);

        public int GetCount(string query);
    }
}
