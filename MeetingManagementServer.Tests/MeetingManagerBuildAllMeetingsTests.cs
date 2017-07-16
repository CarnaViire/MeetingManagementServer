using MeetingManagementServer.Models;
using MeetingManagementServer.Services;
using MeetingManagementServer.Services.Interfaces;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace MeetingManagementServer.Tests
{
    public class MeetingManagerBuildAllMeetingsTests
    {
        [Fact]
        public void ReturnsEmptyArrayForNoPartners()
        {
            var manager = SetupTest(new PartnerDto[0]);

            var result = manager.BuildAllMeetings();

            Assert.Empty(result);
        }

        [Fact]
        public void ReturnsSingleEmptyEventForSinglePartnerWithoutConsecutiveDates()
        {
            var manager = SetupTest(new PartnerDto {
                Name = "John Doe",
                Email = "john.doe@jd.com",
                Country = "USA",
                AvailableDates = new[] { new DateTime(2017, 5, 3), new DateTime(2017, 5, 5) }
            });

            var result = manager.BuildAllMeetings();

            Assert.Equal(1, result.Length);

            var meeting = result.Single();
            Assert.False(meeting.StartDate.HasValue);
            Assert.False(meeting.Attendees.Any());
        }

        [Fact]
        public void ReturnsSingleEventForSinglePartnerWithConsecutiveDates()
        {
            var partnerDto = new PartnerDto
            {
                Name = "John Doe",
                Email = "john.doe@jd.com",
                Country = "USA",
                AvailableDates = new[] { new DateTime(2017, 5, 3), new DateTime(2017, 5, 4) }
            };

            var manager = SetupTest(partnerDto);

            var result = manager.BuildAllMeetings();

            Assert.Equal(1, result.Length);

            var meeting = result.Single();
            Assert.Equal(partnerDto.AvailableDates.First(), meeting.StartDate);
            Assert.Equal(1, meeting.Attendees.Length);
            Assert.Equal(partnerDto.Email, meeting.Attendees.Single().Email);
        }

        private MeetingManager SetupTest(params PartnerDto[] partnerDtos)
        {
            var countries = partnerDtos.Select(p => p.Country)
                .Distinct()
                .Select((c, i) => new Country
                    {
                        Id = i + 1,
                        Name = c
                    })
                .ToDictionary(c => c.Name, c => c);

            var partners = partnerDtos.Select((p, i) => new Partner
                    {
                        Id = i + 1,
                        Name = p.Name,
                        Email = p.Email,
                        Country = countries[p.Country]
                    })
                .ToDictionary(p => p.Email, p => p);

            var availableDates = partnerDtos.SelectMany(p => p.AvailableDates
                .Select(d => new { Email = p.Email, Date = d })
            )
            .Select((d, i) => new AvailableDate
                {
                    Id = i + 1,
                    Partner = partners[d.Email],
                    Date = d.Date
                })
            .ToArray();

            var mPartnerRepository = new Mock<IRepository<Partner>>();
            mPartnerRepository.Setup(r => r.GetAll()).Returns(partners.Values.AsQueryable());

            var mCountryRepository = new Mock<IRepository<Country>>();
            mCountryRepository.Setup(r => r.GetAll()).Returns(countries.Values.AsQueryable());

            var mAvailableDateRepository = new Mock<IRepository<AvailableDate>>();
            mAvailableDateRepository.Setup(r => r.GetAll()).Returns(availableDates.AsQueryable());

            return new MeetingManager(mPartnerRepository.Object, mCountryRepository.Object, mAvailableDateRepository.Object);
        }
    }
}
