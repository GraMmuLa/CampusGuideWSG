using CampusGuideWSG.Context;
using Microsoft.EntityFrameworkCore;

namespace CampusGuideWSG.Helpers.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(Action action)
        {
            action();
            try
            {
                _dbContext.SaveChanges();
            }
            catch(DbUpdateException)
            {
                
            }
        }

        public T ExecuteWithResult<T>(Func<T> func)
        {
            T result = func();
            try
            {
                _dbContext.SaveChanges();

            }
            catch (DbUpdateException)
            {
            }
            return result;
        }
    }
}
