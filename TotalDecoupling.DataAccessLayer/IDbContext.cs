using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalDecoupling.DataAccessLayer
{
    public interface IDbContext
    {
        IQueryable<T> GetData<T>(bool trackingChanges = false) where T : class;

        void Insert<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task SaveAsync();
    }
}
