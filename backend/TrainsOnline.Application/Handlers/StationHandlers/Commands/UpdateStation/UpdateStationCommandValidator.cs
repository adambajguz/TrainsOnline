namespace TrainsOnline.Application.Handlers.StationHandlers.Commands.UpdateStation
{
    using Domain.Entities;
    using FluentValidation;
    using TrainsOnline.Application.Constants;
    using TrainsOnline.Application.Interfaces.UoW;

    public class UpdateStationCommandValidator : AbstractValidator<UpdateStationCommandValidator.Model>
    {
        public UpdateStationCommandValidator(ITrainsOnlineSQLUnitOfWork uow)
        {
            RuleFor(x => x.Data.Id).NotEmpty().Must((request, val, token) =>
            {
                return request.Station != null;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Data.Name).NotEmpty()
                                     .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }

        public class Model
        {
            public UpdateStationRequest Data { get; set; }
            public Station Station { get; set; }

            public Model(UpdateStationRequest data, Station station)
            {
                Data = data;
                Station = station;
            }
        }
    }
}
