using System.ComponentModel.DataAnnotations;

namespace MeetingManagementServer.Models
{
    public class Partner : IEntity
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public Country Country { get; set; }
    }
}
