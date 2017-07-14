using MeetingManagementServer.Models;
using System;

namespace MeetingManagementServer.Dtos
{
    public class Event
    {
        public Country Country { get; set; }

        public DateTime StartDate { get; set; }

        public Partner[] Attendees { get; set; }
    }
}
