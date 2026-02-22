using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Patients.Queries
{
    public record GetAllPatientsQuery : IRequest<List<PatientDto>>;

    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, List<PatientDto>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public GetAllPatientsQueryHandler(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<List<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await _patientRepository.GetAllAsync();
            return _mapper.Map<List<PatientDto>>(patients);
        }
    }
}
