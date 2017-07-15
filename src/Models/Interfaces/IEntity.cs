namespace MeetingManagementServer.Models
{
    /// <summary>
    /// Base interface for all entities (persistent objects)
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Entity id
        /// </summary>
        long Id { get; set; }
    }
}
