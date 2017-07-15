using System;

namespace MeetingManagementServer.Services.Interfaces
{
    /// <summary>
    /// Transaction manager
    /// </summary>
    public interface ITransactionManager
    {
        /// <summary>
        /// Execute the action in database transaction
        /// </summary>
        /// <param name="action">Action to execute</param>
        void InTransaction(Action action);
    }
}
