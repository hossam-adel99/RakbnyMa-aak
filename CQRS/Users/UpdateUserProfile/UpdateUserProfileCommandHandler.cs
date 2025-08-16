using MediatR;
using Microsoft.AspNetCore.Identity;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Services.Interfaces;
using System.Security.Claims;

namespace RakbnyMa_aak.CQRS.Users.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Response<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICloudinaryService _cloudinary;

        public UpdateUserProfileCommandHandler(UserManager<ApplicationUser> userManager, ICloudinaryService cloudinary)
        {
            _userManager = userManager;
            _cloudinary = cloudinary;
        }

        public async Task<Response<string>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Response<string>.Fail("User not found");


                var emailOwner = await _userManager.FindByEmailAsync(dto.Email);
                if (emailOwner != null && emailOwner.Id != user.Id)
                    return Response<string>.Fail("Email is already in use.");

                user.Email = dto.Email;           
                user.PhoneNumber = dto.PhoneNumber;                    
                user.FullName = dto.FullName;
                user.UserName = dto.FullName;
            

            if (dto.Picture != null)
            {
                var uploadedUrl = await _cloudinary.UploadImageAsync(dto.Picture, "users/profile");
                user.Picture = uploadedUrl;
            }

            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Response<string>.Fail(errors);
            }

            return Response<string>.Success(user.Id, "User profile updated successfully.");
        }
    }

}
