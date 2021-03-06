﻿using MeetingManagementServer.Models;
using System;

namespace MeetingManagementServer.Dtos
{
    /// <summary>
    /// Meeting with partners
    /// </summary>
    public class Meeting
    {
        public Country Country { get; set; }

        public DateTime? StartDate { get; set; }

        public Partner[] Attendees { get; set; }
    }
}
