using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Commands.SendNotification
{
    public record SendNotificationCommand(SendNotificationDto NotificationDto) : IRequest<Response<bool>>;

}
