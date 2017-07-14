using System.ComponentModel.DataAnnotations;

namespace MeetingManagementServer.Models
{
    public class Country
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
