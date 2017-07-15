using System;

namespace MeetingManagementServer.Services.Interfaces
{
    public interface ITransactionFactory
    {
        void InTransaction(Action action);

        void Flush();
    }
}
