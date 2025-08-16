using MediatR;
using RakbnyMa_aak.DTOs.UserDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Auth.Queries.GetUserByName
{
    public record GetUserByNameQuery(string Name) : IRequest<Response<UserResponseDto>>;

}
