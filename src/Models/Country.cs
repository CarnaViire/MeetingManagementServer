using System.ComponentModel.DataAnnotations;

namespace MeetingManagementServer.Models
{
    /// <summary>
    /// Country
    /// </summary>
    public class Country : IEntity
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
