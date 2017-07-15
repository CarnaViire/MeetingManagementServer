using MeetingManagementServer.Models;
using MeetingManagementServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MeetingManagementServer.Services.EntityFramework
{
    /// <summary>
    /// Entity Framework repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private EfDataStore _dataStore;

        protected DbSet<TEntity> DbSet => _dataStore.Set<TEntity>();

        public EfRepository(EfDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void DeleteMany(TEntity[] entities)
        {
            DbSet.RemoveRange(entities);
        }

        public TEntity Get(long id)
        {
            return DbSet.SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public void Save(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void SaveMany(TEntity[] entities)
        {
            DbSet.AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateMany(TEntity[] entities)
        {
            DbSet.UpdateRange(entities);
        }
    }
}
