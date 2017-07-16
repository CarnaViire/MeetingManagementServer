using MeetingManagementServer.Models;
using MeetingManagementServer.Services;
using MeetingManagementServer.Services.Interfaces;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace MeetingManagementServer.Tests
{
    /// <summary>
    /// Tests for BuildAllMeeting method of MeetingManager class
    /// </summary>
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
        public void ReturnsSingleEmptyMeetingForSinglePartnerWithoutConsecutiveDates()
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
        public void ReturnsSingleMeetingForSinglePartnerWithConsecutiveDates()
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

        [Fact]
        public void ReturnsEarliestDateWhenSeveralVariantsExists()
        {
            var partnerDto = new PartnerDto
            {
                Name = "John Doe",
                Email = "john.doe@jd.com",
                Country = "USA",
                AvailableDates = new[] {
                    new DateTime(2017, 5, 1),
                    new DateTime(2017, 5, 4),
                    new DateTime(2017, 5, 5),
                    new DateTime(2017, 5, 6),
                    new DateTime(2017, 5, 10),
                    new DateTime(2017, 5, 11)
                }
            };

            var manager = SetupTest(partnerDto);

            var result = manager.BuildAllMeetings();

            Assert.Equal(1, result.Length);
            Assert.Equal(new DateTime(2017, 5, 4), result.Single().StartDate);
        }

        [Fact]
        public void ReturnsMeetingsForAllCountries()
        {
            var partnerDtos = new[] {
                new PartnerDto
                {
                    Name = "John Doe",
                    Email = "john.doe@jd.com",
                    Country = "USA",
                    AvailableDates = new[] { new DateTime(2017, 5, 3), new DateTime(2017, 5, 4) }
                },
                new PartnerDto
                {
                    Name = "Kathy Abrams",
                    Email = "kathy.abrams@ka.com",
                    Country = "UK",
                    AvailableDates = new[] { new DateTime(2017, 5, 3), new DateTime(2017, 5, 4) }
                },
                new PartnerDto
                {
                    Name = "Roy Martin",
                    Email = "roy.martin@rm.com",
                    Country = "CANADA",
                    AvailableDates = new[] { new DateTime(2017, 5, 3), new DateTime(2017, 5, 10) }
                }
            };

            var manager = SetupTest(partnerDtos);

            var result = manager.BuildAllMeetings();

            Assert.Equal(3, result.Length);
            
            Assert.True(result.Any(m => m.Country.Name == "USA"));
            Assert.True(result.Any(m => m.Country.Name == "UK"));
            Assert.True(result.Any(m => m.Country.Name == "CANADA"));
        }

        [Fact]
        public void ReturnsMeetingsForAllCountriesWithSeveralPartnersInCountries()
        {
            var usaPartnerDtos = new[] {
                new PartnerDto
                {
                    Name = "John Doe",
                    Email = "john.doe@jd.com",
                    Country = "USA",
                    AvailableDates = new[] {
                        new DateTime(2017, 5, 1),
                        new DateTime(2017, 5, 4),
                        new DateTime(2017, 5, 5),
                        new DateTime(2017, 5, 6),
                        new DateTime(2017, 5, 10),
                        new DateTime(2017, 5, 11)
                    }
                },
                new PartnerDto
                {
                    Name = "Kathy Abrams",
                    Email = "kathy.abrams@ka.com",
                    Country = "USA",
                    AvailableDates = new[] {
                        new DateTime(2017, 5, 5),
                        new DateTime(2017, 5, 6),
                        new DateTime(2017, 5, 10),
                        new DateTime(2017, 5, 11),
                        new DateTime(2017, 5, 21),
                        new DateTime(2017, 5, 22)
                    }
                },
                new PartnerDto
                {
                    Name = "Amy Dove",
                    Email = "amy.dove@ad.com",
                    Country = "USA",
                    AvailableDates = new[] {
                        new DateTime(2017, 5, 10),
                        new DateTime(2017, 5, 11),
                        new DateTime(2017, 5, 21),
                        new DateTime(2017, 5, 22),
                        new DateTime(2017, 5, 23),
                    }
                },
            };

            var ukPartnerDtos = new[] {
                new PartnerDto
                {
                    Name = "Mary Smith",
                    Email = "mary.smith@ms.com",
                    Country = "UK",
                    AvailableDates = new[] { new DateTime(2017, 5, 3), new DateTime(2017, 5, 4) }
                },
                new PartnerDto
                {
                    Name = "Foo Bar",
                    Email = "foo.bar@fb.com",
                    Country = "UK",
                    AvailableDates = new[] { new DateTime(2017, 5, 4), new DateTime(2017, 5, 6) }
                }
            };

            var canadaPartnerDtos = new[] {
                new PartnerDto
                {
                    Name = "Liam Murphy",
                    Email = "liam.murphy@lm.com",
                    Country = "CANADA",
                    AvailableDates = new[] {
                        new DateTime(2017, 5, 4),
                        new DateTime(2017, 5, 5),
                        new DateTime(2017, 5, 6),
                        new DateTime(2017, 5, 10),
                        new DateTime(2017, 5, 11)
                    }
                },
                new PartnerDto
                {
                    Name = "Anna Baz",
                    Email = "anna.baz@ab.com",
                    Country = "CANADA",
                    AvailableDates = new[] {
                        new DateTime(2017, 5, 1),
                        new DateTime(2017, 5, 4),
                        new DateTime(2017, 5, 5)
                    }
                },
                new PartnerDto
                {
                    Name = "Roy Martin",
                    Email = "roy.martin@rm.com",
                    Country = "CANADA",
                    AvailableDates = new[] {
                        new DateTime(2017, 5, 6),
                        new DateTime(2017, 5, 10),
                        new DateTime(2017, 5, 11)
                    }
                },
                new PartnerDto
                {
                    Name = "Fox Brown",
                    Email = "fox.brown@fb.com",
                    Country = "CANADA",
                    AvailableDates = new[] { new DateTime(2017, 5, 3), new DateTime(2017, 5, 10) }
                }
            };

            var manager = SetupTest(usaPartnerDtos.Concat(ukPartnerDtos).Concat(canadaPartnerDtos).ToArray());

            var result = manager.BuildAllMeetings();

            Assert.Equal(3, result.Length);

            var usaResult = result.Single(m => m.Country.Name == "USA");
            var ukResult = result.Single(m => m.Country.Name == "UK");
            var canadaResult = result.Single(m => m.Country.Name == "CANADA");

            Assert.Equal(new DateTime(2017, 5, 10), usaResult.StartDate);
            Assert.Equal(3, usaResult.Attendees.Length);

            Assert.Equal(new DateTime(2017, 5, 3), ukResult.StartDate);
            Assert.Equal(1, ukResult.Attendees.Length);

            Assert.Equal(new DateTime(2017, 5, 4), canadaResult.StartDate);
            Assert.Equal(2, canadaResult.Attendees.Length);
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
