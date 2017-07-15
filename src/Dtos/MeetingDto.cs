using System;

namespace MeetingManagementServer.Models
{
    /// <summary>
    /// Meeting with partners DTO
    /// </summary>
    public class MeetingDto
    {
        public string Country { get; set; }

        public DateTime? StartDate { get; set; }

        public AttendeeDto[] Attendees { get; set; }
    }
}
