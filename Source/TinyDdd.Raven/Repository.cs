using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using SwissKnife;

namespace TinyDdd.Raven
{
    public abstract class Repository<T> : IRepository<T> where T : Entity, IAggregateRoot
    {
        private readonly IDocumentSession _documentSession;

        protected Repository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public void AddOrUpdate(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(IAggregateRoot entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(IAggregateRoot entity)
        {
            throw new NotImplementedException();
        }
        
        public Option<T> GetById(Guid id)
        {
            return _documentSession.Load<T>(id);
        }

        public Option<T> GetOne(System.Linq.Expressions.Expression<Func<T, bool>> criteria)
        {
            return _documentSession.Query<T>().FirstOrDefault(criteria);
        }

        public IEnumerable<T> GetAll()
        {
            return _documentSession.Query<T>();
        }

        public IEnumerable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> criteria)
        {
            return _documentSession.Query<T>().Where(criteria);
        }
    }
}
