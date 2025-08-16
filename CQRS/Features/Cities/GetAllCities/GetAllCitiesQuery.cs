using AutoMapper;
using MediatR;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.DTOs.Shared;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Cities.GetAllCities
{
    public record GetAllCitiesQuery(GetAllRequestDto RequestDto)
     : IRequest<Response<PaginatedResult<CityDto>>>;



}
