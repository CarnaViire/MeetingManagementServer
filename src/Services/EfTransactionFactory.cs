using MeetingManagementServer.Services.Interfaces;
using System;

namespace MeetingManagementServer.Services
{
    public class EfTransactionFactory : ITransactionFactory
    {
        private EfDataStore _efDataStore;

        public EfTransactionFactory(EfDataStore efDataStore)
        {
            _efDataStore = efDataStore;
        }

        public void InTransaction(Action action)
        {
            using (var transaction = _efDataStore.Database.BeginTransaction())
            {
                try
                {
                    action();

                    Flush();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void Flush()
        {
            _efDataStore.SaveChanges();
        }
    }
}
