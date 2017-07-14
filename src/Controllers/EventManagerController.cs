using Microsoft.AspNetCore.Mvc;
using MeetingManagementServer.Services;
using System.Linq;
using MeetingManagementServer.Models;

namespace MeetingManagementServer.Controllers
{
    /// <summary>
    /// Controller to accessc the event manager functions through API 
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class EventManagerController : Controller
    {
        private EfDataStore _efDataStore;
        private EventManager _eventManager;

        public EventManagerController(EventManager eventManager, EfDataStore efDataStore)
        {
            _eventManager = eventManager;
            _efDataStore = efDataStore;
        }

        /// <summary>
        /// Get events of 2 day length with maximal attendance for all countries
        /// </summary>
        [HttpGet]
        public IActionResult BuildEvents()
        {
            var result = _eventManager.BuildEvents().Select(e => new EventDto
            {
                Country = e.Country.Name,
                Attendees = e.Attendees.Select(a => new AttendeeDto { Id = a.Id, Name = a.Name, Email = a.Email }).ToArray(),
                StartDate = e.StartDate
            }).ToArray();

            return new JsonResult(result);
        }

        /// <summary>
        /// Get the event of 2 day length with maximal attendance for a specific country
        /// </summary>
        [HttpGet]
        public IActionResult BuildEvent([FromQuery]string country)
        {
            var countryEntity = _efDataStore.Countries.SingleOrDefault(c => c.Name == country);
            if (countryEntity == null)
            {
                return NotFound("Contry is not found");
            }

            var countryEvent = _eventManager.BuildEvent(countryEntity);
            
            var result = new EventDto
            {
                Country = countryEvent.Country.Name,
                Attendees = countryEvent.Attendees.Select(a => new AttendeeDto { Id = a.Id, Name = a.Name, Email = a.Email }).ToArray(),
                StartDate = countryEvent.StartDate
            };

            return new JsonResult(result);
        }
    }
}
