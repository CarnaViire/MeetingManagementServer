using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MeetingManagementServer.Services;
using MeetingManagementServer.Models;
using System.Linq.Expressions;
using System;

namespace MeetingManagementServer.Controllers
{
    [Route("api/[controller]")]
    public class PartnersController : Controller
    {
        private EfDataStore _dataStore;

        private Expression<Func<Partner, PartnerDto>> ToPartnerDtoExpression => p => new PartnerDto
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            Country = p.Country.Name,
            AvailableDates = _dataStore.AvailableDates.Where(d => d.Partner.Id == p.Id).Select(d => d.Date).ToArray()
        };

        public PartnersController(EfDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        // GET api/partners
        [HttpGet]
        public IEnumerable<PartnerDto> Get()
        {
            return _dataStore.Partners.Select(ToPartnerDtoExpression).ToArray();
        }

        // GET api/partners/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var partnerDto = _dataStore.Partners.Where(x => x.Id == id).Select(ToPartnerDtoExpression).SingleOrDefault();
            if (partnerDto == null)
            {
                return NotFound();
            }
            return new ObjectResult(partnerDto);
        }

        // POST api/partners
        [HttpPost]
        public IActionResult Post([FromBody]PartnerDto partnerDto)
        {
            if (partnerDto == null)
            {
                return BadRequest("Partner is not specified correctly");
            }

            if (partnerDto.Id != 0)
            {
                return BadRequest("To update an existing partner, use PUT request");
            }
            
            var partner = ToPartner(partnerDto);

            var validationResult = Validate(partner).ToArray();
            if (validationResult.Any())
            {
                return BadRequest(string.Join(Environment.NewLine, validationResult));
            }

            if (partner.Country.Id == 0)
            {
                _dataStore.Countries.Add(partner.Country);
            }
            
            var availableDates = ToAvailableDates(partnerDto, partner);
            
            _dataStore.Partners.Add(partner);
            if (availableDates != null && availableDates.Any())
            {
                _dataStore.AvailableDates.AddRange(availableDates);
            }

            _dataStore.SaveChanges();

            return Ok(ToPartnerDto(partner, availableDates));
        }

        // PUT api/partners/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]PartnerDto partnerDto)
        {
            if (partnerDto == null)
            {
                return BadRequest("Partner is not specified correctly");
            }

            if (string.IsNullOrWhiteSpace(partnerDto.Country))
            {
                return BadRequest("Country is not specified correctly");
            }

            if (id == 0)
            {
                return BadRequest("To add a new partner, use POST request");
            }

            partnerDto.Id = id;
            
            if (!_dataStore.Partners.Any(x => x.Id == id))
            {
                return NotFound();
            }

            var partner = ToPartner(partnerDto);

            var validationResult = Validate(partner).ToArray();
            if (validationResult.Any())
            {
                return BadRequest(string.Join(Environment.NewLine, validationResult));
            }

            if (partner.Country.Id == 0)
            {
                _dataStore.Countries.Add(partner.Country);
            }

            var dates = _dataStore.AvailableDates.Where(d => d.Partner.Id == partnerDto.Id).ToArray();
            if (dates.Any())
            {
                _dataStore.AvailableDates.RemoveRange(dates);
            }
            var availableDates = ToAvailableDates(partnerDto, partner);

            _dataStore.Partners.Update(partner);
            _dataStore.AvailableDates.AddRange(availableDates);

            _dataStore.SaveChanges();

            return Ok(ToPartnerDto(partner, availableDates));
        }

        // DELETE api/partners/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var partner = _dataStore.Partners.SingleOrDefault(x => x.Id == id);
            if (partner == null)
            {
                return NotFound();
            }

            var availableDates = _dataStore.AvailableDates.Where(d => d.Partner.Id == id).ToArray();
            if (availableDates.Any())
            {
                _dataStore.AvailableDates.RemoveRange(availableDates);
            }

            _dataStore.Partners.Remove(partner);
            _dataStore.SaveChanges();
            return Ok(ToPartnerDto(partner, availableDates));
        }

        private PartnerDto ToPartnerDto(Partner partner, AvailableDate[] availableDates)
        {
            return new PartnerDto
            {
                Id = partner.Id,
                Name = partner.Name,
                Email = partner.Email,
                Country = partner.Country.Name,
                AvailableDates = availableDates.Select(d => d.Date).ToArray()
            };
        }

        private Partner ToPartner(PartnerDto dto)
        {
            var partner = new Partner
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                Country = _dataStore.Countries.SingleOrDefault(c => c.Name == dto.Country.ToUpperInvariant()) ?? new Country { Name = dto.Country.ToUpperInvariant() }
            };

            return partner;
        }

        private AvailableDate[] ToAvailableDates(PartnerDto dto, Partner partner)
        {
            if (dto.AvailableDates == null)
            {
                return null;
            }

            return dto.AvailableDates
                      .Distinct()
                      .Select(d => new AvailableDate { Partner = partner, Date = d })
                      .ToArray();
        }

        private IEnumerable<string> Validate(Partner partner)
        {
            if (_dataStore.Partners.Any(x => x.Id != partner.Id && x.Email == partner.Email))
            {
                yield return "Email is not unique";
            }
        }
    }
}
