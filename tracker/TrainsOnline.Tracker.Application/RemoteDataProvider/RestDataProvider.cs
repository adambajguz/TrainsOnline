namespace TrainsOnline.Desktop.Domain.RemoteDataProvider
{
    using System;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using RestSharp;
    using TrainsOnline.Tracker.Application.DTO;
    using TrainsOnline.Tracker.Application.DTO.Analytics;
    using TrainsOnline.Tracker.Application.DTO.Authentication;
    using TrainsOnline.Tracker.Application.DTO.EntityAuditLog;
    using TrainsOnline.Tracker.Application.DTO.Route;
    using TrainsOnline.Tracker.Application.DTO.RouteLog;
    using TrainsOnline.Tracker.Application.DTO.RouteReport;
    using TrainsOnline.Tracker.Application.DTO.Station;
    using TrainsOnline.Tracker.Application.DTO.User;
    using TrainsOnline.Tracker.Application.Exceptions;
    using TrainsOnline.Tracker.Application.Extensions;

    public class RestDataProvider
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
