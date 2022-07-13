using AutoMapper;
using FinancialChat.Application.Common.Mappings;
using FinancialChat.Domain.Entities;

namespace FinancialChat.Application.ChatRooms.Queries.GetChatRooms;

public class ChatRoomDto : IMapFrom<ChatRoom>
{
    public ChatRoomDto()
    {
        Messages = new List<ChatMessageDto>();
    }

    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public bool Global { get; set; }

    public IList<ChatMessageDto> Messages { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ChatRoom, ChatRoomDto>()
            .ForMember(d => d.Messages, opt => opt.MapFrom(s => s.Messages.OrderByDescending(o => o.Created).Take(ChatRoom.MESSAGES_LIMIT)));
    }
}

