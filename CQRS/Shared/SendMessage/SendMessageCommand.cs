using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Shared.SendMessage
{
    public record SendChatMessageCommand(string SenderId, SendMessageDto Dto)
        : IRequest<Response<string>>;
}