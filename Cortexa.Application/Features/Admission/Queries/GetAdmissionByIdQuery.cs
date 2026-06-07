using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Admission.Queries
{
    public record GetAdmissionByIdQuery(string Id) : IRequest<ResultDto<AdmissionDto>>;

    public class GetAdmissionByIdQueryHandler : IRequestHandler<GetAdmissionByIdQuery, ResultDto<AdmissionDto>>
    {
        private readonly IAdmissionRepository _admissionRepository;
        private readonly IMapper _mapper;

        public GetAdmissionByIdQueryHandler(IAdmissionRepository admissionRepository, IMapper mapper)
        {
            _admissionRepository = admissionRepository;
            _mapper = mapper;
        }

        public async Task<ResultDto<AdmissionDto>> Handle(GetAdmissionByIdQuery request, CancellationToken cancellationToken)
        {
            var admission = await _admissionRepository.GetByIdAsync(request.Id);
            if (admission == null)
            {
                return ResultDto<AdmissionDto>.Failure("Admission not found.");
            }
            return ResultDto<AdmissionDto>.SuccessResult(_mapper.Map<AdmissionDto>(admission), "Admission retrieved successfully.");
        }
    }
}