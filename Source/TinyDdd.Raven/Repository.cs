using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Client;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

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
            Argument.IsNotNull(entity, "entity");

            _documentSession.Store(entity);
        }

        public void Delete(T entity)
        {
            Argument.IsNotNull(entity, "entity");

            _documentSession.Delete( entity );
        }

        public void AddOrUpdate(IAggregateRoot entity)
        {
            Argument.IsNotNull(entity, "entity");
            Argument.Is<T>((object)entity, "entity");

            AddOrUpdate((T)entity);
        }

        public void Delete(IAggregateRoot entity)
        {
            Argument.IsNotNull(entity, "entity");
            Argument.Is<T>((object)entity, "entity");

            Delete((T)entity);
        }
        
        public Option<T> GetById(Guid id)
        {
            return _documentSession.Load<T>(id);
        }

        public Option<T> GetOne(Expression<Func<T, bool>> criteria)
        {
            return _documentSession.Query<T>().FirstOrDefault(criteria);
        }

        public IEnumerable<T> GetAll()
        {
            return _documentSession.Query<T>();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> criteria)
        {
            return _documentSession.Query<T>().Where(criteria);
        }
    }
}
