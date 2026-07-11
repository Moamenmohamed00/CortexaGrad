using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Diagnostics;
using Cortexa.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Diagnostics.Queries
{
    public record GetImagingStudiesQuery(string AdmissionId) : IRequest<List<ImagingDto>>;

    public class GetImagingStudiesQueryHandler : IRequestHandler<GetImagingStudiesQuery, List<ImagingDto>>
    {
        private readonly IImagingRepository _imagingRepository;
        private readonly IMapper _mapper;

        public GetImagingStudiesQueryHandler(IImagingRepository imagingRepository, IMapper mapper)
        {
            _imagingRepository = imagingRepository;
            _mapper = mapper;
        }

        public async Task<List<ImagingDto>> Handle(GetImagingStudiesQuery request, CancellationToken cancellationToken)
        {
            var imaging = await _imagingRepository.GetImagingByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<ImagingDto>>(imaging);
        }
    }
}
