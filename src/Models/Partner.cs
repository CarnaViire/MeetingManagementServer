using System.ComponentModel.DataAnnotations;

namespace MeetingManagementServer.Models
{
    public class Partner
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
