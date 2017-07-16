using Microsoft.AspNetCore.Mvc;
using System.Linq;
using MeetingManagementServer.Models;
using MeetingManagementServer.Services.Interfaces;

namespace MeetingManagementServer.Controllers
{
    /// <summary>
    /// Controller to access the meeting manager functions through API 
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class MeetingController : Controller
    {
        private IRepository<Country> _countryRepository;
        private IMeetingManager _meetingManager;

        public MeetingController(IMeetingManager meetingManager, IRepository<Country> countryRepository)
        {
            _meetingManager = meetingManager;
            _countryRepository = countryRepository;
        }

        /// <summary>
        /// Get meetings of 2 day length with maximal attendance for all countries
        /// </summary>
        [HttpGet]
        public IActionResult BuildAll()
        {
            var result = _meetingManager.BuildAllMeetings().Select(e => new MeetingDto
            {
                Country = e.Country.Name,
                Attendees = e.Attendees.Select(a => new AttendeeDto { Id = a.Id, Name = a.Name, Email = a.Email }).ToArray(),
                StartDate = e.StartDate
            }).ToArray();

            return new JsonResult(result);
        }

        /// <summary>
        /// Get the meeting of 2 day length with maximal attendance for a specific country
        /// </summary>
        [HttpGet]
        public IActionResult Build([FromQuery]string country)
        {
            var countryEntity = _countryRepository.GetAll().SingleOrDefault(c => c.Name == country);
            if (countryEntity == null)
            {
                return NotFound("Country is not found");
            }

            var meeting = _meetingManager.BuildMeeting(countryEntity);
            
            var meetingDto = new MeetingDto
            {
                Country = meeting.Country.Name,
                Attendees = meeting.Attendees.Select(a => new AttendeeDto { Id = a.Id, Name = a.Name, Email = a.Email }).ToArray(),
                StartDate = meeting.StartDate
            };

            return new JsonResult(meetingDto);
        }
    }
}
