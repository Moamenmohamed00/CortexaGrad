using AutoMapper;
using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.Nurses
{
    public record GetAllNursesByDepartmentQuery(string Department) : IRequest<ResultDto<List<NurseDto>>>;


    public class GetAllNursesByDepartmentQueryHandler : IRequestHandler<GetAllNursesByDepartmentQuery, ResultDto<List<NurseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllNursesByDepartmentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResultDto<List<NurseDto>>> Handle(GetAllNursesByDepartmentQuery request, CancellationToken cancellationToken)
        {
            var nurses = await _unitOfWork.Nurses.GetByDepartmentAsync(request.Department);
            if (nurses == null || nurses.Count == 0)
            {
                return ResultDto<List<NurseDto>>.Failure($"No nurses found in department: {request.Department}");
            }
            var nurseDtos = _mapper.Map<List<NurseDto>>(nurses);
            return ResultDto<List<NurseDto>>.SuccessResult(nurseDtos, "Nurses retrieved successfully.");
        }
    }
}