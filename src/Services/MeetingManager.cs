using MeetingManagementServer.Dtos;
using MeetingManagementServer.Models;
using MeetingManagementServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingManagementServer.Services
{
    /// <summary>
    /// Meeting manager to plan meetings with partners in countries
    /// </summary>
    public class MeetingManager : IMeetingManager
    {
        private IRepository<Partner> _partnerRepository;

        private IRepository<Country> _countryRepository;

        private IRepository<AvailableDate> _availableDateRepository;

        public MeetingManager(IRepository<Partner> partnerRepository, 
            IRepository<Country> countryRepository,
            IRepository<AvailableDate> availableDateRepository)
        {
            _partnerRepository = partnerRepository;
            _countryRepository = countryRepository;
            _availableDateRepository = availableDateRepository;
        }

        /// <summary>
        /// Get meetings of 2 day length with maximal attendance for all countries
        /// </summary>
        public Meeting[] BuildAllMeetings()
        {
            var partnerGroups = _partnerRepository.GetAll()
                .GroupBy(p => p.Country.Id)
                .ToDictionary(g => g.Key, g => g.ToArray());

            var countryIds = partnerGroups.Keys.ToArray();
            var countries = _countryRepository.GetAll()
                .Where(c => countryIds.Contains(c.Id))
                .ToDictionary(c => c.Id, c => c);

            var availableDatesByPartner = _availableDateRepository.GetAll()
                .GroupBy(d => d.Partner.Id)
                .ToDictionary(g => g.Key, g => g.Select(d => d.Date).ToArray());

            return partnerGroups.Select(g => BuildMeeting(countries[g.Key], g.Value, availableDatesByPartner)).ToArray();
        }

        /// <summary>
        /// Get the meeting of 2 day length with maximal attendance for a specific country
        /// </summary>
        public Meeting BuildMeeting(Country country)
        {
            var partners = _partnerRepository.GetAll().Where(p => p.Country.Id == country.Id).ToArray();

            var availableDatesByPartner = _availableDateRepository.GetAll()
                .Where(d => d.Partner.Country.Id == country.Id)
                .GroupBy(d => d.Partner.Id)
                .ToDictionary(g => g.Key, g => g.Select(d => d.Date).ToArray());

            return BuildMeeting(country, partners, availableDatesByPartner);
        }

        private Meeting BuildMeeting(Country country, Partner[] partners, Dictionary<long, DateTime[]> availableDatesByPartner)
        {
            var partnersByDate = new Dictionary<DateTime, HashSet<Partner>>();

            foreach (var partner in partners)
            {
                if (!availableDatesByPartner.TryGetValue(partner.Id, out var availableDates))
                {
                    continue;
                }

                foreach (var date in availableDates)
                {
                    if(!partnersByDate.TryGetValue(date, out var partnerSet))
                    {
                        partnerSet = new HashSet<Partner>();
                        partnersByDate[date] = partnerSet;
                    }

                    partnerSet.Add(partner);
                }
            }

            var maxAttendeeSet = new HashSet<Partner>();
            DateTime? startDate = null;

            var allDates = partnersByDate.Keys.OrderBy(d => d).ToArray();

            for (var i = 0; i < allDates.Length - 1; ++i)
            {
                var date = allDates[i];
                var nextDate = allDates[i + 1];

                if (date.AddDays(1) != nextDate)
                {
                    continue;
                }

                var attendees = new HashSet<Partner>(partnersByDate[date]);
                attendees.IntersectWith(partnersByDate[nextDate]);

                if (attendees.Count > maxAttendeeSet.Count)
                {
                    maxAttendeeSet = attendees;
                    startDate = date;
                }
            }

            return new Meeting { Attendees = maxAttendeeSet.ToArray(), Country = country, StartDate = startDate };
        }
    }
}
