using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TotalDecoupling.DataAccessLayer
{
    public class DataContext : DbContext, IDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public IQueryable<T> GetData<T>(bool trackingChanges = false) where T : class
        {
            var set = Set<T>();
            return trackingChanges ? set.AsTracking() : set.AsNoTrackingWithIdentityResolution();
        }

        public void Insert<T>(T entity) where T : class => Set<T>().Add(entity);

        public void Delete<T>(T entity) where T : class => Set<T>().Remove(entity);

        public Task SaveAsync()
            => SaveChangesAsync();
    }
}
