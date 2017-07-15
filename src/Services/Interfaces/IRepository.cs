using MeetingManagementServer.Models;
using System.Linq;

namespace MeetingManagementServer.Services.Interfaces
{
    /// <summary>
    /// Repository to perform CRUD operations on entities
    /// </summary>
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> GetAll();

        TEntity Get(long id);

        void Save(TEntity entity);

        void SaveMany(TEntity[] entities);

        void Update(TEntity entity);

        void UpdateMany(TEntity[] entities);

        void Delete(TEntity entity);

        void DeleteMany(TEntity[] entities);
    }
}
