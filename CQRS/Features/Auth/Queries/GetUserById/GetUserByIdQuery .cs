using MediatR;
using RakbnyMa_aak.DTOs.UserDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Auth.Queries.GetUserById
{
    public record GetUserByIdQuery(string Id) : IRequest<Response<UserResponseDto>>;

}
