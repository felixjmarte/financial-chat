using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinancialChat.Application.Common.Interfaces;
using FinancialChat.Application.Common.DTO;
using FinancialChat.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Application.ChatRooms.Queries.GetChatRooms;

public record GetChatRoomsQuery : IRequest<ChatRoomVm>
{
    public string? ChatRoomCode { get; set; }
}

public class GetChatRoomsQueryHandler : IRequestHandler<GetChatRoomsQuery, ChatRoomVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetChatRoomsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<ChatRoomVm> Handle(GetChatRoomsQuery request, CancellationToken cancellationToken)
    {
        return new ChatRoomVm
        {
            ChatRooms = await _context.ChatRooms
                .Where(r => !string.IsNullOrEmpty(request.ChatRoomCode) ? r.Code!.ToLower() == request.ChatRoomCode.ToLower() : true)
                .Where(r => r.Global || r.Users.Any(u => u.UserId == _currentUserService.UserId))
                .AsNoTracking()
                .ProjectTo<ChatRoomDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
        };    
    }
}