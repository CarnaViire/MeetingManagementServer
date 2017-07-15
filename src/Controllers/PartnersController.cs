using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MeetingManagementServer.Models;
using System.Linq.Expressions;
using System;
using MeetingManagementServer.Services.Interfaces;

namespace MeetingManagementServer.Controllers
{
    /// <summary>
    /// Controller to provide REST API to partners
    /// </summary>
    [Route("api/[controller]")]
    public class PartnersController : Controller
    {
        private IRepository<Partner> _partnerRepository;

        private IRepository<Country> _countryRepository;

        private IRepository<AvailableDate> _availableDateRepository;

        private ITransactionFactory _transactionFactory;

        private Expression<Func<Partner, PartnerDto>> ToPartnerDtoExpression => p => new PartnerDto
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            Country = p.Country.Name,
            AvailableDates = _availableDateRepository.GetAll().Where(d => d.Partner.Id == p.Id).Select(d => d.Date).ToArray()
        };

        public PartnersController(IRepository<Partner> partnerRepository, 
            IRepository<Country> countryRepository, 
            IRepository<AvailableDate> availableDateRepository,
            ITransactionFactory transactionFactory)
        {
            _partnerRepository = partnerRepository;
            _countryRepository = countryRepository;
            _availableDateRepository = availableDateRepository;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        /// Get all partners
        /// </summary>
        [HttpGet]
        public IEnumerable<PartnerDto> Get()
        {
            return _partnerRepository.GetAll().Select(ToPartnerDtoExpression).ToArray();
        }

        /// <summary>
        /// Get specific partner
        /// </summary>
        /// <param name="id">Partner id</param>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var partnerDto = _partnerRepository.GetAll().Where(x => x.Id == id).Select(ToPartnerDtoExpression).SingleOrDefault();
            if (partnerDto == null)
            {
                return NotFound();
            }
            return new ObjectResult(partnerDto);
        }

        /// <summary>
        /// Save a new partner
        /// </summary>
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
            
            var availableDates = ToAvailableDates(partnerDto, partner);

            _transactionFactory.InTransaction(() => {
                if (partner.Country.Id == 0)
                {
                    _countryRepository.Save(partner.Country);
                }

                _partnerRepository.Save(partner);
                if (availableDates != null && availableDates.Any())
                {
                    _availableDateRepository.SaveMany(availableDates);
                }
            });

            return Ok(ToPartnerDto(partner, availableDates));
        }

        /// <summary>
        /// Update the specific partner
        /// </summary>
        /// <param name="id">Partner id</param>
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
            
            if (!_partnerRepository.GetAll().Any(x => x.Id == id))
            {
                return NotFound();
            }

            var partner = ToPartner(partnerDto);
            var availableDates = ToAvailableDates(partnerDto, partner);

            var validationResult = Validate(partner).ToArray();
            if (validationResult.Any())
            {
                return BadRequest(string.Join(Environment.NewLine, validationResult));
            }

            _transactionFactory.InTransaction(() =>
            {
                if (partner.Country.Id == 0)
                {
                    _countryRepository.Save(partner.Country);
                }

                var dates = _availableDateRepository.GetAll().Where(d => d.Partner.Id == partnerDto.Id).ToArray();
                if (dates.Any())
                {
                    _availableDateRepository.DeleteMany(dates);
                }

                _partnerRepository.Update(partner);
                _availableDateRepository.SaveMany(availableDates);
            });

            return Ok(ToPartnerDto(partner, availableDates));
        }

        /// <summary>
        /// Delete the specific partner
        /// </summary>
        /// <param name="id">Partner id</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var partner = _partnerRepository.Get(id);
            if (partner == null)
            {
                return NotFound();
            }

            var availableDates = _availableDateRepository.GetAll().Where(d => d.Partner.Id == id).ToArray();

            _transactionFactory.InTransaction(() =>
            {
                if (availableDates.Any())
                {
                    _availableDateRepository.DeleteMany(availableDates);
                }

                _partnerRepository.Delete(partner);
            });
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
                Country = _countryRepository.GetAll().SingleOrDefault(c => c.Name == dto.Country.ToUpperInvariant()) ?? new Country { Name = dto.Country.ToUpperInvariant() }
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
                      .Select(d => new AvailableDate { Partner = partner, Date = d.Date })
                      .ToArray();
        }

        private IEnumerable<string> Validate(Partner partner)
        {
            if (_partnerRepository.GetAll().Any(x => x.Id != partner.Id && x.Email == partner.Email))
            {
                yield return "Email is not unique";
            }
        }
    }
}
