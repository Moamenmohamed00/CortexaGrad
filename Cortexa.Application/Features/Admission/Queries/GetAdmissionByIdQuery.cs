using AutoMapper;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Patient;
using Cortexa.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Admission.Queries
{
    public record GetAdmissionByIdQuery(string Id) : IRequest<ResultDto<PatientAdmissionDto>>;

    public class GetAdmissionByIdQueryHandler : IRequestHandler<GetAdmissionByIdQuery, ResultDto<PatientAdmissionDto>>
    {
        private readonly IAdmissionRepository _admissionRepository;
        private readonly IMapper _mapper;

        public GetAdmissionByIdQueryHandler(IAdmissionRepository admissionRepository, IMapper mapper)
        {
            _admissionRepository = admissionRepository;
            _mapper = mapper;
        }

        public async Task<ResultDto<PatientAdmissionDto>> Handle(GetAdmissionByIdQuery request, CancellationToken cancellationToken)
        {
            var admission = await _admissionRepository.GetByIdAsync(request.Id);
            if (admission == null)
            {
                return ResultDto<PatientAdmissionDto>.Failure("Admission not found.");
            }
            return ResultDto<PatientAdmissionDto>.SuccessResult(_mapper.Map<PatientAdmissionDto>(admission), "Admission retrieved successfully.");
        }
    }
}