using MeetingManagementServer.Dtos;
using MeetingManagementServer.Models;
using MeetingManagementServer.Services.Interfaces;
using System;
using System.Linq;

namespace MeetingManagementServer.Services
{
    public class MeetingManager : IMeetingManager
    {
        private IRepository<Partner> _partnerRepository;

        private IRepository<Country> _countryRepository;

        public MeetingManager(IRepository<Partner> partnerRepository, IRepository<Country> countryRepository)
        {
            _partnerRepository = partnerRepository;
            _countryRepository = countryRepository;
        }

        public Meeting[] BuildMeetings()
        {
            return new[] { new Meeting { Attendees = _partnerRepository.GetAll().Take(3).ToArray(), Country = _countryRepository.GetAll().First(), StartDate = DateTime.Today } };
        }

        public Meeting BuildMeeting(Country country)
        {
            //var dates = new Dictionary<DateTime, HashSet<Partner>>

            return new Meeting { Attendees = _partnerRepository.GetAll().Take(3).ToArray(), Country = _countryRepository.GetAll().First(), StartDate = DateTime.Today };
        }
    }
}
