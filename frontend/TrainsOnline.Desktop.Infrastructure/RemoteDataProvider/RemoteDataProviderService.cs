namespace TrainsOnline.Desktop.Domain.RemoteDataProvider
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider;
    using TrainsOnline.Desktop.Domain.DTO;
    using TrainsOnline.Desktop.Domain.DTO.Analytics;
    using TrainsOnline.Desktop.Domain.DTO.Authentication;
    using TrainsOnline.Desktop.Domain.DTO.EntityAuditLog;
    using TrainsOnline.Desktop.Domain.DTO.Route;
    using TrainsOnline.Desktop.Domain.DTO.Station;
    using TrainsOnline.Desktop.Domain.DTO.Ticket;
    using TrainsOnline.Desktop.Domain.DTO.User;
    using TrainsOnline.Desktop.Domain.RemoteDataProvider.Interfaces;
    using TrainsOnline.Desktop.Infrastructure.Helpers;

    public class RemoteDataProviderService : IRemoteDataProviderService
    {
        private bool useLocalUrl;
        private string token;

        private IMapper Mapper { get; set; }

        public bool UseLocalUrl
        {
            get => useLocalUrl;
            set
            {
                useLocalUrl = value;
                RestProvider.UseLocalUrl = value;
            }
        }

        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Token);
        protected string Token
        {
            get => token; private set
            {
                token = value;
                RestProvider.SetToken(value);
            }
        }

        private JwtTokenHelper JwtHelper { get; }
        private RestDataProvider RestProvider { get; }

        private IDataProvider DataProvider => RestProvider;

        public RemoteDataProviderService(IMapper mapper)
        {
            Mapper = mapper;

            JwtHelper = new JwtTokenHelper();
            RestProvider = new RestDataProvider();
        }

        #region User
        public Guid GetUserId()
        {
            return JwtHelper.GetUserIdFromToken(Token);
        }

        public bool HasRole(string role)
        {
            return JwtHelper.IsRoleInToken(Token, role);
        }

        public bool HasAnyOfRoles(params string[] roles)
        {
            return JwtHelper.IsAnyOfRolesInToken(Token, roles);
        }

        public async Task<bool> Login(string email, string password)
        {
            JwtTokenModel jwtTokenModel = await DataProvider.Login(new LoginRequest
            {
                Email = email ?? string.Empty,
                Password = password ?? string.Empty
            });

            Token = jwtTokenModel?.Token;

            return IsAuthenticated;
        }

        public void Logout()
        {
            Token = string.Empty;
        }

        public async Task<Guid> Register(CreateUserRequest data)
        {
            IdResponse response = await DataProvider.Register(data);

            return response.Id;
        }

        public async Task<GetUserDetailsResponse> GetCurrentUser()
        {
            return await DataProvider.GetCurrentUser();
        }
        #endregion

        #region Station
        public async Task<GetStationDetailsResponse> GetStation(Guid id)
        {
            return await DataProvider.GetStation(id);
        }

        public async Task<GetStationsListResponse> GetStations()
        {
            return await DataProvider.GetStations();
        }
        #endregion

        #region Route
        public async Task<GetRouteDetailsResponse> GetRoute(Guid id)
        {
            return await DataProvider.GetRoute(id);
        }

        public async Task<GetRoutesListResponse> GetFilteredRoutes(GetFilteredRoutesListRequest data)
        {
            return await DataProvider.GetFilteredRoutes(data);
        }

        public async Task<GetRoutesListResponse> GetRoutes()
        {
            return await DataProvider.GetRoutes();
        }
        #endregion

        #region Ticket
        public async Task<Guid> CreateTicket(CreateTicketRequest data)
        {
            IdResponse response = await DataProvider.CreateTicket(data);

            return response.Id;
        }

        public async Task<Guid> CreateTicketForCurrentUser(Guid routeId)
        {
            IdResponse response = await DataProvider.CreateTicket(new CreateTicketRequest
            {
                UserId = GetUserId(),
                RouteId = routeId
            });

            return response.Id;
        }

        public async Task<GetTicketDetailsResponse> GetTicket(Guid id)
        {
            return await DataProvider.GetTicket(id);
        }

        public async Task<GetTicketDocumentResponse> GetTicketDocument(Guid id)
        {
            return await DataProvider.GetTicketDocument(id);
        }

        public async Task<GetUserTicketsListResponse> GetCurrentUserTickets()
        {
            return await DataProvider.GetCurrentUserTickets();
        }

        public async Task UpdateUser(UpdateUserRequest data)
        {
            await DataProvider.UpdateUser(data);
        }

        public async Task ChangePassword(string currentPassword, string newPassword)
        {
            await DataProvider.ChangePassword(new ChangePasswordRequest
            {
                UserId = GetUserId(),
                OldPassword = currentPassword ?? "",
                NewPassword = newPassword ?? ""
            });
        }
        #endregion

        #region Analytics
        public async Task<GetAnalyticsRecordsListResponse> GetAnalytics()
        {
            return await DataProvider.GetAnalytics();
        }
        #endregion

        #region EntityAutitLogs
        public async Task<GetEntityAuditLogsListResponse> GetEntityAudtiLogs()
        {
            return await DataProvider.GetEntityAudtiLogs();
        }
        #endregion
    }
}
