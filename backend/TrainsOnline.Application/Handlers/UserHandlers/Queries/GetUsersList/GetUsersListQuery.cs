namespace TrainsOnline.Application.Handlers.UserHandlers.Queries.GetUsersList
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;

    public class GetUsersListQuery : IRequest<GetUsersListResponse>
    {
        public GetUsersListQuery()
        {

        }

        public class Handler : IRequestHandler<GetUsersListQuery, GetUsersListResponse>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;

            public Handler(ITrainsOnlineSQLUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetUsersListResponse> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
            {
                return new GetUsersListResponse
                {
                    Users = await _uow.UsersRepository.ProjectToAsync<GetUsersListResponse.UserLookupModel>(cancellationToken: cancellationToken)
                };
            }
        }
    }
}

