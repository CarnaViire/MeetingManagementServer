using System;

namespace MeetingManagementServer.Models
{
    public class PartnerDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public DateTime[] AvailableDates { get; set; }
    }
}
