using AutoMapper;
using FinancialChat.Application.Common.Mappings;
using FinancialChat.Domain.Entities;

namespace FinancialChat.Application.Common.DTO;

public class ChatMessageDto : BaseDto, IMapFrom<ChatMessage>
{
    public string? ChatRoomCode { get; set; }

    public string? Message { get; set; }

    public string? UserId { get; set; }

    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ChatMessage, ChatMessageDto>()
            .ForMember(d => d.ChatRoomCode, opt => opt.MapFrom(s => s.ChatRoom != null ? s.ChatRoom.Code : string.Empty));
    }
}