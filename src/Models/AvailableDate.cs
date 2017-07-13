using System;
using System.ComponentModel.DataAnnotations;

namespace MeetingManagementServer.Models
{
    public class AvailableDate
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public Partner Partner { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
