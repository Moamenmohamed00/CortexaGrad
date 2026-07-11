using MediatR;
using AutoMapper;
using Cortexa.Application.Dtos.Diagnostics;
using Cortexa.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Diagnostics.Queries
{
    public record GetLabOrdersQuery(string AdmissionId) : IRequest<List<LabOrderDto>>;

    public class GetLabOrdersQueryHandler : IRequestHandler<GetLabOrdersQuery, List<LabOrderDto>>
    {
        private readonly ILabRepository _labRepository;
        private readonly IMapper _mapper;

        public GetLabOrdersQueryHandler(ILabRepository labRepository, IMapper mapper)
        {
            _labRepository = labRepository;
            _mapper = mapper;
        }

        public async Task<List<LabOrderDto>> Handle(GetLabOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _labRepository.GetOrdersByAdmissionIdAsync(request.AdmissionId);
            return _mapper.Map<List<LabOrderDto>>(orders);
        }
    }
}
