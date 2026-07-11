using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.Nurses
{
    public record GetAllNursesQuery() : IRequest<ResultDto<List<NurseDto>>>;

    public class GetAllNursesQueryHandler : IRequestHandler<GetAllNursesQuery, ResultDto<List<NurseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllNursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResultDto<List<NurseDto>>> Handle(GetAllNursesQuery request, CancellationToken cancellationToken)
        {
            var nurses = await _unitOfWork.Nurses.GetAllAsync();
            if (nurses == null)
            {
                return ResultDto<List<NurseDto>>.Failure("No nurses found.");
            }
            var nurseDtos = _mapper.Map<List<NurseDto>>(nurses);
            return ResultDto<List<NurseDto>>.SuccessResult(nurseDtos, "Nurses retrieved successfully.");
        }
    }
}