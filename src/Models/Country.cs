using System.ComponentModel.DataAnnotations;

namespace MeetingManagementServer.Models
{
    public class Country : IEntity
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
