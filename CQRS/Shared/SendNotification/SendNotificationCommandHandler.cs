using MediatR;
using Microsoft.AspNetCore.Identity;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Services.Interfaces;

namespace RakbnyMa_aak.CQRS.Commands.SendNotification
{
    public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Response<bool>>
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SendNotificationCommandHandler(
            INotificationService notificationService,
            UserManager<ApplicationUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public async Task<Response<bool>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.NotificationDto;

            var user = await _userManager.FindByIdAsync(dto.SenderUserId);
            if (user == null)
                return Response<bool>.Fail("المستخدم غير موجود.");

            await _notificationService.SendNotificationAsync(dto.ReceiverId, user, dto.Message);

            return Response<bool>.Success(true, "تم إرسال الإشعار بنجاح.");
        }
    }
}
