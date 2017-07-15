using System;

namespace MeetingManagementServer.Models
{
    public class MeetingDto
    {
        public string Country { get; set; }

        public DateTime StartDate { get; set; }

        public AttendeeDto[] Attendees { get; set; }
    }
}
