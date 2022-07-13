using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinancialChat.Application.Common.Interfaces;
using FinancialChat.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Application.ChatRooms.Queries.GetChatRooms;

public record GetChatRoomsQuery : IRequest<List<ChatRoomsVm>>
{
    public string? ChatRoomCode { get; set; }
}

public class GetChatRoomsQueryHandler : IRequestHandler<GetChatRoomsQuery, List<ChatRoomsVm>>
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

    public async Task<List<ChatRoomsVm>> Handle(GetChatRoomsQuery request, CancellationToken cancellationToken)
    {
        return await _context.ChatRooms
            .Where(r => !string.IsNullOrEmpty(request.ChatRoomCode) ? r.Code!.ToLower() == request.ChatRoomCode.ToLower() : true)
            .Where(r => r.Global || r.Users.Any(u => u.UserId == _currentUserService.UserId))
            .AsNoTracking()
            .ProjectTo<ChatRoomDto>(_mapper.ConfigurationProvider)
            .OrderByDescending(r => r.Messages.Max(m => m.Created))
            .Select(dto => new ChatRoomsVm()
            {
                Name = dto.Name,
                Code = dto.Code,
                Global = dto.Global,
                Messages = dto.Messages
            })
            .ToListAsync(cancellationToken);
    }
}