using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using IUGOCare.Application.Common.Exceptions;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Common;
using IUGOCare.Domain.Common.Constants;
using IUGOCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IUGOCare.Application.Observations.Queries.GetPatientObservations
{
    public class GetPatientObservationsQuery : IRequest<PatientObservationsVm>
    {
        public Guid ClinicPatientId { get; set; }
        public DateTimeOffset EffectiveDateStart { get; set; }
        public DateTimeOffset EffectiveDateEnd { get; set; }
        public string[] ObservationCodes { get; set; } = new string[] { };
        public string UnitSystem;
}

    public class GetObservationsQueryHandler : IRequestHandler<GetPatientObservationsQuery, PatientObservationsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeOffset _dateTimeOffset;

        public GetObservationsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IDateTimeOffset dateTimeOffset)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<PatientObservationsVm> Handle(GetPatientObservationsQuery request, CancellationToken cancellationToken)
        {
            var clinicPatientIds = GetClinicPatientIds();
            if (! clinicPatientIds.Contains(request.ClinicPatientId))
            {
                var vf = new ValidationFailure("ClinicPatientId", "This patient does not have access to these results.", request.ClinicPatientId);
                throw new ValidationException(new List<ValidationFailure> { vf });
            }

            string patientTimeZone = _context.ClinicPatients.FirstOrDefault(cp => cp.ClinicPatientId == request.ClinicPatientId)?.TimeZone;

            IQueryable<Observation> patientObservationsQuery = _context.Observations
                .Include(d => d.ObservationsData)
                .Where(o => o.EffectiveDate >= request.EffectiveDateStart.ToUniversalTime()
                    && o.EffectiveDate <= request.EffectiveDateEnd.ToUniversalTime().AddHours(23).AddMinutes(59).AddSeconds(59)
                    && request.ObservationCodes.Contains(o.ObservationCode)
                    && o.ClinicPatientId == request.ClinicPatientId);

            var observations = await patientObservationsQuery
                    .OrderByDescending(o => o.EffectiveDate)
                    .ThenByDescending(o => o.Created)
                    .ToListAsync(cancellationToken);

            var observationDtos = observations.Select(o => _mapper.Map<PatientObservationDto>(o)).ToList();
            observationDtos.ForEach(dto => dto.EffectiveDate = _dateTimeOffset.ConvertToLocal(dto.EffectiveDate, patientTimeZone));

            if(request.UnitSystem == UnitSystems.ImperialSystem)
            {
                observationDtos.ForEach(dto => dto.ObservationsData = ConvertedValuesAndUnitsToImperial(dto.ObservationsData));
            }
            return new PatientObservationsVm { Observations = observationDtos };
        }

        private List<Guid> GetClinicPatientIds()
        {
            var patient = _context.Patients
                    .Include(p => p.Clinics)
                    .FirstOrDefault(p => p.Auth0Id == _currentUserService.UserId);
            return patient?.Clinics?.Select(cp => cp.ClinicPatientId).ToList() ?? new List<Guid>();
        }

        private IList<PatientObservationDataDto> ConvertedValuesAndUnitsToImperial(IList<PatientObservationDataDto> observationDataDtos)
        {
            IList<PatientObservationDataDto> observationData = new List<PatientObservationDataDto>();
            foreach (var dto in observationDataDtos)
            {
                var conversion = UnitConversionUtility.ConvertToImperialUnit(dto.Unit, dto.Value);

                var newObservationData = new PatientObservationDataDto()
                {
                   
                    ObservationCode = dto.ObservationCode,
                    Unit = conversion.DestinationUnit,
                    Value = conversion.ConvertedValue
                };

                observationData.Add(newObservationData);
            }

            return observationData;
        }
    }
}
