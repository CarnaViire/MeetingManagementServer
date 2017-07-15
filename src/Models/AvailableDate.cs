using System;
using System.ComponentModel.DataAnnotations;

namespace MeetingManagementServer.Models
{
    /// <summary>
    /// A date available for a meeting
    /// </summary>
    public class AvailableDate : IEntity
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public Partner Partner { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
