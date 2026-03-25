using AutoMapper;
using Cortexa.Application.Dtos.Rooms;
using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.Rooms.Queries
{
    public record GetRoomsQuery() : IRequest<List<RoomDto>>;
    public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, List<RoomDto>>
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;

        public GetRoomsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitofwork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<RoomDto>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
        {
            var Rooms = await _unitofwork.Rooms.GetRoomsWithBedsAvailableAsync();
            return _mapper.Map<List<RoomDto>>(Rooms);
        }
    }
}
