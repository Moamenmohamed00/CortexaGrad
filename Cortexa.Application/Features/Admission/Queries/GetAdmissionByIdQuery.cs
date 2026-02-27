using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Admission.Queries
{
    public record GetAdmissionByIdQuery(string Id) : IRequest<AdmissionDto>;

    public class GetAdmissionByIdQueryHandler : IRequestHandler<GetAdmissionByIdQuery, AdmissionDto>
    {
        private readonly IAdmissionRepository _admissionRepository;
        private readonly IMapper _mapper;

        public GetAdmissionByIdQueryHandler(IAdmissionRepository admissionRepository, IMapper mapper)
        {
            _admissionRepository = admissionRepository;
            _mapper = mapper;
        }

        public async Task<AdmissionDto> Handle(GetAdmissionByIdQuery request, CancellationToken cancellationToken)
        {
            var admission = await _admissionRepository.GetByIdAsync(request.Id);
            return _mapper.Map<AdmissionDto>(admission);
        }
    }
}