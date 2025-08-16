using MediatR;
using Microsoft.AspNetCore.Identity;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Services.Interfaces;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Admin.RejectDriver
{
    public class RejectDriverCommandHandler : IRequestHandler<RejectDriverCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public RejectDriverCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Response<bool>> Handle(RejectDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = await _unitOfWork.DriverRepository.GetByUserIdAsync(request.DriverId);

            if (driver == null)
                return Response<bool>.Fail("لم يتم العثور على السائق", statusCode: 404);

            _unitOfWork.DriverRepository.Delete(driver);

            var user = await _userManager.FindByIdAsync(request.DriverId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return Response<bool>.Fail("فشل في حذف المستخدم: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                await _emailService.SendEmailAsync(
                    user.Email,
                    "رفض طلب السائق",
                    $@"
                        <div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: auto; border: 1px solid #ddd; border-radius: 10px; padding: 20px; background-color: #fdf2f2;"">
                            <div style=""text-align: center;"">
                                <h2 style=""color: #c62828;"">🚫 تم رفض طلبك كسائق</h2>
                            </div>

                            <p style=""font-size: 16px; color: #333;"">
                                عزيزي <strong>{user.FullName}</strong>،
                            </p>

                            <p style=""font-size: 16px; color: #333;"">
                                نأسف لإبلاغك بأنه قد تم رفض طلبك للتسجيل كسائق في منصة <strong>ركبني معاك</strong>.
                            </p>

                            <p style=""font-size: 16px; color: #333;"">
                                السبب: {request.Reason ?? "غير محدد"}
                            </p>

                            <p style=""font-size: 14px; color: gray; text-align: center;"">
                                فريق <strong>ركبني معاك</strong> يتمنى لك التوفيق 💚
                            </p>
                        </div>"
                );
            }

            await _unitOfWork.CompleteAsync();
            return Response<bool>.Success(true, "تم رفض تسجيل السائق وتم حذف الحساب.");
        }
    }
}
