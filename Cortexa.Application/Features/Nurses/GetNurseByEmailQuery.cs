using AutoMapper;
using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.Nurses
{
    public record GetNurseByEmailQuery(string Email) : IRequest<ResultDto<NurseDto>>;

    public class GetNurseByEmailQueryHandler : IRequestHandler<GetNurseByEmailQuery, ResultDto<NurseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetNurseByEmailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResultDto<NurseDto>> Handle(GetNurseByEmailQuery request, CancellationToken cancellationToken)
        {
            var nurse = await _unitOfWork.Nurses.GetByEmailAsync(request.Email);
            if (nurse == null)
            {
                return ResultDto<NurseDto>.Failure($"No nurse found with email: {request.Email}");
            }
            var nurseDto = _mapper.Map<NurseDto>(nurse);
            return ResultDto<NurseDto>.SuccessResult(nurseDto, "Nurse retrieved successfully.");
        }
    }
}
