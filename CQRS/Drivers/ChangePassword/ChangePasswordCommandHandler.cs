using MediatR;
using Microsoft.AspNetCore.Identity;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.CQRS.Drivers.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Response<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangePasswordHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Response<string>.Fail("لم يتم العثور على المستخدم.");

            var result = await _userManager.ChangePasswordAsync(user, request.Dto.OldPassword, request.Dto.NewPassword);
            return result.Succeeded
                ? Response<string>.Success("تم تحديث كلمة المرور بنجاح.")
                : Response<string>.Fail("خطأ: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
