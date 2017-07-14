using MeetingManagementServer.Dtos;
using MeetingManagementServer.Models;
using System;
using System.Linq;

namespace MeetingManagementServer.Services
{
    public class EventManager
    {
        private EfDataStore _dataStore;

        public EventManager(EfDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public Event[] BuildEvents()
        {
            return new[] { new Event { Attendees = _dataStore.Partners.Take(3).ToArray(), Country = _dataStore.Countries.First(), StartDate = DateTime.Now } };
        }

        public Event BuildEvent(Country country)
        {
            //var dates = new Dictionary<DateTime, HashSet<Partner>>

            return new Event { Attendees = _dataStore.Partners.Take(3).ToArray(), Country = _dataStore.Countries.First(), StartDate = DateTime.Now };
        }
    }
}
