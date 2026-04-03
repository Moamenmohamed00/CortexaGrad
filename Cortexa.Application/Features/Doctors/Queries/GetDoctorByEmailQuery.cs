using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Patients.Queries
{
    public record GetDoctorByEmailQuery(string Email) : IRequest<DoctorDto>;

    public class GetDoctorByEmailQueryHandler : IRequestHandler<GetDoctorByEmailQuery, DoctorDto>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public GetDoctorByEmailQueryHandler(IDoctorRepository doctorRepository , IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<DoctorDto> Handle(GetDoctorByEmailQuery request, CancellationToken cancellationToken)
        {
            var Doctor = await _doctorRepository.GetByEmailAsync(request.Email);
            return _mapper.Map<DoctorDto>(Doctor);
        }
    }
}
