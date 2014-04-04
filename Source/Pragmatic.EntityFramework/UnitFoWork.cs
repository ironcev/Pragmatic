using System.Data.Entity;

namespace Pragmatic.EntityFramework
{
    public class UnitFoWork : Pragmatic.UnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitFoWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private void AttachIfNeeded(Entity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
                _dbContext.Set(entity.GetType()).Attach(entity);
        }


        protected override void MarkEntityAsAdded(Entity entity)
        {
            AttachIfNeeded(entity);

            _dbContext.Entry(entity).State = EntityState.Added;
        }
        
        protected override void MarkEntityAsUpdated(Entity entity)
        {
            AttachIfNeeded(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        protected override void MarkEntityAsDeleted(Entity entity)
        {
            AttachIfNeeded(entity);

            _dbContext.Set(entity.GetType()).Remove(entity);
        }

        protected override void SaveMarkedChanges()
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
