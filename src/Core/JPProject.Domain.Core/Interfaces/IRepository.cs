using System;

namespace JPProject.Domain.Core.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        void Update(TEntity obj);
        void Remove<T>(T id);
        void Remove(TEntity obj);
    }
}
