using MediatR;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateCityInGovernorate;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripBusinessRules;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Trip.Orchestrators.ValidateTripBusinessLogic
{
    public class ValidateTripBusinessLogicOrchestratorHandler : IRequestHandler<ValidateTripBusinessLogicOrchestrator, Response<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public ValidateTripBusinessLogicOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(ValidateTripBusinessLogicOrchestrator request, CancellationToken cancellationToken)
        {
            var dto = request.TripDto;
            // Step 1: Validate trip business rules 
            var validation = await _mediator.Send(new ValidateTripBusinessRulesCommand(dto));
            if (!validation.IsSucceeded)
                return Response<bool>.Fail(validation.Message);

            // Check driver IsApproved
            var driver = await _unitOfWork.DriverRepository.GetByUserIdAsync(dto.DriverId);
            if (driver == null)
                return Response<bool>.Fail("لم يتم العثور على السائق", statusCode: 404);

            if (!driver.IsApproved)
                return Response<bool>.Fail("السائق لم تتم الموافقة عليه من قبل المسؤول بعد", statusCode: 403);

            // Step 2: Ensure the "FromCity" actually belongs to the "FromGovernorate"
            var fromCityValidation = await _mediator.Send(new ValidateCityInGovernorateCommand
            (
                dto.FromCityId,
                dto.FromGovernorateId
            ));
            if (!fromCityValidation.IsSucceeded)
                return Response<bool>.Fail(fromCityValidation.Message);

            // Step 3 Ensure the "ToCity" actually belongs to the "ToGovernorate"
            var toCityValidation = await _mediator.Send(new ValidateCityInGovernorateCommand
            (
                dto.ToCityId,
                dto.ToGovernorateId
            ));
            if (!toCityValidation.IsSucceeded)
                return Response<bool>.Fail(toCityValidation.Message);

            return Response<bool>.Success(true);
        }
    }

}
