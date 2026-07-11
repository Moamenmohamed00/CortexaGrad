using AutoMapper;
using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.Doctors.Queries
{
    public record GetAllDoctorsQuery() : IRequest<List<DoctorDto>>;

    public class GetAllDoctorsQueryHandler : IRequestHandler<GetAllDoctorsQuery, List<DoctorDto>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public GetAllDoctorsQueryHandler(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<List<DoctorDto>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return _mapper.Map<List<DoctorDto>>(doctors);
        }
    }
}
