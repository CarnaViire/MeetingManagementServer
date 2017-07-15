using MeetingManagementServer.Services.Interfaces;
using System;

namespace MeetingManagementServer.Services.EntityFramework
{
    /// <summary>
    /// Entity Framework transaction manager
    /// </summary>
    public class EfTransactionManager : ITransactionManager
    {
        private EfDataStore _efDataStore;

        public EfTransactionManager(EfDataStore efDataStore)
        {
            _efDataStore = efDataStore;
        }

        /// <summary>
        /// Execute the action in database transaction
        /// </summary>
        /// <param name="action">Action to execute</param>
        public void InTransaction(Action action)
        {
            using (var transaction = _efDataStore.Database.BeginTransaction())
            {
                try
                {
                    action();

                    _efDataStore.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }
    }
}
