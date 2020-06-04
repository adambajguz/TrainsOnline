namespace TrainsOnline.Desktop.Domain.RemoteDataProvider
{
    using System;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using RestSharp;
    using TrainsOnline.Desktop.Application.Exceptions;
    using TrainsOnline.Desktop.Domain.DTO;
    using TrainsOnline.Desktop.Domain.DTO.Analytics;
    using TrainsOnline.Desktop.Domain.DTO.Authentication;
    using TrainsOnline.Desktop.Domain.DTO.EntityAuditLog;
    using TrainsOnline.Desktop.Domain.DTO.Route;
    using TrainsOnline.Desktop.Domain.DTO.RouteLog;
    using TrainsOnline.Desktop.Domain.DTO.RouteReport;
    using TrainsOnline.Desktop.Domain.DTO.Station;
    using TrainsOnline.Desktop.Domain.DTO.Ticket;
    using TrainsOnline.Desktop.Domain.DTO.User;
    using TrainsOnline.Desktop.Domain.Extensions;
    using TrainsOnline.Desktop.Domain.RemoteDataProvider.Interfaces;

    public class RestDataProvider : IDataProvider
    {
        private const string ApiUrl = "https://genericapi.francecentral.cloudapp.azure.com/api/";
        private const string ApiUrlLocal = "http://localhost:2137/api";

        private bool useLocalUrl;
        public bool UseLocalUrl
        {
            get => useLocalUrl; set
            {
                useLocalUrl = value;
                Client = new RestClient(UseLocalUrl ? ApiUrlLocal : ApiUrl);
            }
        }

        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Token);
        protected string Token { get; private set; }

        protected RestClient Client { get; private set; }

        public RestDataProvider()
        {
            Client = new RestClient(UseLocalUrl ? ApiUrlLocal : ApiUrl);
        }

        public void SetToken(string token)
        {
            Token = token;
        }

        private static void CheckResponseErrors(IRestResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                string str = Encoding.UTF8.GetString(response.RawBytes);
                throw new RemoteDataException(str);
            }
        }

        public async Task<JwtTokenModel> Login(LoginRequest data)
        {
            RestRequest request = new RestRequest("user/login", DataFormat.Json);
            request.AddJsonBody(data);

            IRestResponse<JwtTokenModel> response = await Client.ExecutePostAsync<JwtTokenModel>(request);
            CheckResponseErrors(response);

            JwtTokenModel jwtTokenModel = response.Data;
            Token = jwtTokenModel?.Token;

            return jwtTokenModel;
        }

        public void Logout()
        {
            Token = string.Empty;
        }

        public async Task<IdResponse> Register(CreateUserRequest data)
        {
            RestRequest request = new RestRequest("user/create", DataFormat.Json);
            request.AddJsonBody(data);

            IRestResponse<IdResponse> response = await Client.ExecutePostAsync<IdResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }

        public async Task<GetUserDetailsResponse> GetCurrentUser()
        {
            RestRequest request = new RestRequest("user/get-current", DataFormat.Json);
            request.AddBearerAuthentication(Token);

            return await Client.GetAsync<GetUserDetailsResponse>(request);
        }

        public async Task<GetStationDetailsResponse> GetStation(Guid id)
        {
            RestRequest request = new RestRequest("station/get/{id}", DataFormat.Json);
            request.AddParameter("id", id, ParameterType.UrlSegment);

            IRestResponse<GetStationDetailsResponse> response = await Client.ExecuteGetAsync<GetStationDetailsResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }

        public async Task<GetStationsListResponse> GetStations()
        {
            RestRequest request = new RestRequest("station/get-all", DataFormat.Json);

            IRestResponse<GetStationsListResponse> response = await Client.ExecuteGetAsync<GetStationsListResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }

        public async Task<GetRouteDetailsResponse> GetRoute(Guid id)
        {
            RestRequest request = new RestRequest("route/get/{id}", DataFormat.Json);
            request.AddParameter("id", id, ParameterType.UrlSegment);

            IRestResponse<GetRouteDetailsResponse> response = await Client.ExecuteGetAsync<GetRouteDetailsResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }

        public async Task<GetRoutesListResponse> GetRoutes()
        {
            RestRequest request = new RestRequest("route/get-all", DataFormat.Json);

            IRestResponse<GetRoutesListResponse> response = await Client.ExecuteGetAsync<GetRoutesListResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }

        public async Task<GetRoutesListResponse> GetFilteredRoutes(GetFilteredRoutesListRequest data)
        {
            RestRequest request = new RestRequest("route/get-filtered", DataFormat.Json);
            request.AddJsonBody(data);

            IRestResponse<GetRoutesListResponse> response = await Client.ExecutePostAsync<GetRoutesListResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }

        public async Task<IdResponse> CreateTicket(CreateTicketRequest data)
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            RestRequest request = new RestRequest("ticket/create", DataFormat.Json);
            request.AddJsonBody(data)
                   .AddBearerAuthentication(Token);

            IRestResponse<IdResponse> response = await Client.ExecutePostAsync<IdResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }

        public async Task<GetTicketDetailsResponse> GetTicket(Guid id)
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            RestRequest request = new RestRequest("ticket/get/{id}", DataFormat.Json);
            request.AddParameter("id", id, ParameterType.UrlSegment)
                   .AddBearerAuthentication(Token);

            IRestResponse<GetTicketDetailsResponse> response = await Client.ExecuteGetAsync<GetTicketDetailsResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }

        public async Task<GetTicketDocumentResponse> GetTicketDocument(Guid id)
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            RestRequest request = new RestRequest("ticket/get-document/{id}", DataFormat.Json);
            request.AddParameter("id", id, ParameterType.UrlSegment)
                   .AddBearerAuthentication(Token);

            IRestResponse<GetTicketDocumentResponse> response = await Client.ExecuteGetAsync<GetTicketDocumentResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }

        public async Task<GetUserTicketsListResponse> GetCurrentUserTickets()
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            RestRequest request = new RestRequest("ticket/get-all-current-user", DataFormat.Json);
            request.AddBearerAuthentication(Token);

            IRestResponse<GetUserTicketsListResponse> response = await Client.ExecuteGetAsync<GetUserTicketsListResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }

        public async Task UpdateUser(UpdateUserRequest data)
        {
            if (!IsAuthenticated)
            {
                return;
            }

            RestRequest request = new RestRequest("user/update", DataFormat.Json);
            request.AddJsonBody(data)
                   .AddBearerAuthentication(Token);

            IRestResponse response = await Client.ExecuteAsync(request, Method.PUT);
            CheckResponseErrors(response);

            return;
        }

        public async Task ChangePassword(ChangePasswordRequest data)
        {
            if (!IsAuthenticated)
            {
                return;
            }

            RestRequest request = new RestRequest("user/change-password", DataFormat.Json);
            request.AddJsonBody(data)
                   .AddBearerAuthentication(Token);

            IRestResponse response = await Client.ExecuteAsync(request, Method.PUT);
            CheckResponseErrors(response);

            return;
        }

        #region Analytics
        public async Task<GetAnalyticsRecordsListResponse> GetAnalytics()
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            RestRequest request = new RestRequest("analytics-record/get-all", DataFormat.Json);
            request.AddBearerAuthentication(Token);

            IRestResponse<GetAnalyticsRecordsListResponse> response = await Client.ExecuteGetAsync<GetAnalyticsRecordsListResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }
        #endregion

        #region EntityAudtiLogs
        public async Task<GetEntityAuditLogsListResponse> GetEntityAudtiLogs()
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            RestRequest request = new RestRequest("entity-audit-log/get-all", DataFormat.Json);
            request.AddBearerAuthentication(Token);

            IRestResponse<GetEntityAuditLogsListResponse> response = await Client.ExecuteGetAsync<GetEntityAuditLogsListResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }
        #endregion

        #region RouteLog
        public async Task<GetRouteLogsListResponse> GetEntityRouteLogs()
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            RestRequest request = new RestRequest("route-log/get-all", DataFormat.Json);
            request.AddBearerAuthentication(Token);

            IRestResponse<GetRouteLogsListResponse> response = await Client.ExecuteGetAsync<GetRouteLogsListResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }
        #endregion

        #region RouteReport
        public async Task<GetRouteReportsListResponse> GetEntityRouteReports()
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            RestRequest request = new RestRequest("route-report/get-all", DataFormat.Json);
            request.AddBearerAuthentication(Token);

            IRestResponse<GetRouteReportsListResponse> response = await Client.ExecuteGetAsync<GetRouteReportsListResponse>(request);
            CheckResponseErrors(response);

            return response.Data;
        }
        #endregion
    }
}
