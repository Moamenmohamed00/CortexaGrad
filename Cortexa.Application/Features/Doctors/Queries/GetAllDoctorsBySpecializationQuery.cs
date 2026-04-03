using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Patients.Queries
{
    public record GetAllDoctorsBySpecializationQuery(string Specialization) : IRequest<List<DoctorDto>>;

    public class GetAllDoctorsBySpecializationQueryHandler : IRequestHandler<GetAllDoctorsBySpecializationQuery, List<DoctorDto>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public GetAllDoctorsBySpecializationQueryHandler(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<List<DoctorDto>> Handle(GetAllDoctorsBySpecializationQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _doctorRepository.GetBySpecializationAsync(request.Specialization);
            return _mapper.Map<List<DoctorDto>>(doctors);
        }
    }
   
}
