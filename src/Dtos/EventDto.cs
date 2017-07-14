using System;

namespace MeetingManagementServer.Models
{
    public class EventDto
    {
        public string Country { get; set; }

        public DateTime StartDate { get; set; }

        public AttendeeDto[] Attendees { get; set; }
    }
}
