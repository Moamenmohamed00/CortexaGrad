using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Patients.Queries
{
    public record GetPatientByIdQuery(string Id) : IRequest<PatientDto>;

    public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, PatientDto>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public GetPatientByIdQueryHandler(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<PatientDto> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetByIdAsync(request.Id);
            return _mapper.Map<PatientDto>(patient);
        }
    }
}
