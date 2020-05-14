namespace TrainsOnline.Desktop.Domain.RemoteDataProvider
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;
    using RestSharp;
    using TrainsOnline.Desktop.Application.Extensions;
    using TrainsOnline.Desktop.Common.Json;
    using TrainsOnline.Desktop.Domain.DTO;
    using TrainsOnline.Desktop.Domain.DTO.Authentication;
    using TrainsOnline.Desktop.Domain.DTO.Route;
    using TrainsOnline.Desktop.Domain.DTO.Station;
    using TrainsOnline.Desktop.Domain.DTO.Ticket;
    using TrainsOnline.Desktop.Domain.DTO.User;
    using TrainsOnline.Desktop.Domain.RemoteDataProvider.Interfaces;
    using SOAPS = Infrastructure.Services.SoapServices;

    public class SoapDataProvider : IDataProvider
    {
        private const string ApiUrl = "https://genericapi.francecentral.cloudapp.azure.com/soap-api";
        private const string ApiUrlLocal = "http://localhost:2137/soap-api";

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

        public SoapDataProvider()
        {
            Client = new RestClient(ApiUrl);
        }

        public void SetToken(string token)
        {
            Token = token;
        }

        #region User
        public async Task<JwtTokenModel> Login(LoginRequest data)
        {
            try
            {
                SOAPS.Authentication.LoginRequest request = new SOAPS.Authentication.LoginRequest
                {
                    Email = data.Email,
                    Password = data.Password
                };
                SOAPS.Authentication.AuthenticationSoapEndpointServiceClient client = new SOAPS.Authentication.AuthenticationSoapEndpointServiceClient();
                SOAPS.Authentication.LoginResponse r = await client.LoginAsync(new SOAPS.Authentication.LoginRequest1(request));

                JwtTokenModel jwtTokenModel = await DeserializeCustom<JwtTokenModel>(r.LoginResult);

                Token = jwtTokenModel?.Token;

                return jwtTokenModel;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Logout()
        {
            Token = string.Empty;
        }

        public async Task<IdResponse> Register(CreateUserRequest data)
        {
            try
            {
                SOAPS.User.CreateUserRequest request = new SOAPS.User.CreateUserRequest
                {
                    Email = data.Email,
                    Name = data.Name,
                    Surname = data.Surname,
                    Address = data.Address,
                    Password = data.Password,
                    IsAdmin = data.IsAdmin,
                    PhoneNumber = data.PhoneNumber
                };
                SOAPS.User.UserSoapEndpointServiceClient client = new SOAPS.User.UserSoapEndpointServiceClient();
                SOAPS.User.CreateUserResponse r = await client.CreateUserAsync(new SOAPS.User.CreateUserRequest1(request));

                return await DeserializeCustom<IdResponse>(r.CreateUserResult);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GetUserDetailsResponse> GetCurrentUser()
        {
            try
            {
                SOAPS.User.UserSoapEndpointServiceClient client = new SOAPS.User.UserSoapEndpointServiceClient();

                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Bearer " + Token;

                OperationContext context = new OperationContext(client.InnerChannel);
                using (new OperationContextScope(context))
                {
                    context.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    SOAPS.User.GetCurrentUserDetailsResponse data = await client.GetCurrentUserDetailsAsync(new SOAPS.User.GetCurrentUserDetailsRequest());

                    return await DeserializeCustom<GetUserDetailsResponse>(data.GetCurrentUserDetailsResult);
                }

                //using (new OperationContextScope(client.InnerChannel))
                //{
                //    // Add a SOAP Header to an outgoing request
                //    MessageHeader<string> aMessageHeader = MessageHeader.CreateHeader("UserInfo", "http://tempuri.org", "");
                //    OperationContext.Current.OutgoingMessageHeaders.Add(aMessageHeader);
                //}
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Station
        public async Task<GetStationDetailsResponse> GetStation(Guid id)
        {
            try
            {
                SOAPS.Station.IdRequest request = new SOAPS.Station.IdRequest
                {
                    Id = id.ToString()
                };
                SOAPS.Station.StationSoapEndpointServiceClient client = new SOAPS.Station.StationSoapEndpointServiceClient();
                SOAPS.Station.GetStationDetailsResponse1 data = await client.GetStationDetailsAsync(new SOAPS.Station.GetStationDetailsRequest(request));

                return await DeserializeCustom<GetStationDetailsResponse>(data.GetStationDetailsResult);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GetStationsListResponse> GetStations()
        {
            try
            {
                SOAPS.Station.StationSoapEndpointServiceClient client = new SOAPS.Station.StationSoapEndpointServiceClient();
                SOAPS.Station.GetStationsListResponse1 data = await client.GetStationsListAsync(new SOAPS.Station.GetStationsListRequest());

                return await DeserializeCustom<GetStationsListResponse>(data.GetStationsListResult);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        public async Task<GetRouteDetailsResponse> GetRoute(Guid id)
        {
            try
            {
                SOAPS.Route.IdRequest request = new SOAPS.Route.IdRequest
                {
                    Id = id.ToString()
                };
                SOAPS.Route.RouteSoapEndpointServiceClient client = new SOAPS.Route.RouteSoapEndpointServiceClient();
                SOAPS.Route.GetRouteDetailsResponse1 data = await client.GetRouteDetailsAsync(new SOAPS.Route.GetRouteDetailsRequest(request));

                return await DeserializeCustom<GetRouteDetailsResponse>(data.GetRouteDetailsResult);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GetRoutesListResponse> GetFilteredRoutes(GetFilteredRoutesListRequest data)
        {
            try
            {
                SOAPS.Route.GetFilteredRoutesListRequest request = new SOAPS.Route.GetFilteredRoutesListRequest
                {
                    FromPattern = data.FromPattern,
                    ToPattern = data.ToPattern,
                    MaximumTicketPrice = data.MaximumTicketPrice
                };

                SOAPS.Route.RouteSoapEndpointServiceClient client = new SOAPS.Route.RouteSoapEndpointServiceClient();
                SOAPS.Route.GetFilteredRoutesListResponse r = await client.GetFilteredRoutesListAsync(new SOAPS.Route.GetFilteredRoutesListRequest1(request));

                return await DeserializeCustom<GetRoutesListResponse>(r.GetFilteredRoutesListResult);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GetRoutesListResponse> GetRoutes()
        {
            try
            {
                SOAPS.Route.RouteSoapEndpointServiceClient client = new SOAPS.Route.RouteSoapEndpointServiceClient();
                SOAPS.Route.GetRoutesListResponse1 data = await client.GetRoutesListAsync(new SOAPS.Route.GetRoutesListRequest());

                return await DeserializeCustom<GetRoutesListResponse>(data.GetRoutesListResult);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IdResponse> CreateTicket(CreateTicketRequest data)
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            try
            {
                SOAPS.Ticket.CreateTicketRequest request = new SOAPS.Ticket.CreateTicketRequest
                {
                    RouteId = data.RouteId.ToString(),
                    UserId = data.UserId.ToString()
                };
                SOAPS.Ticket.TicketSoapEndpointServiceClient client = new SOAPS.Ticket.TicketSoapEndpointServiceClient();

                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Bearer " + Token;

                OperationContext context = new OperationContext(client.InnerChannel);
                using (new OperationContextScope(context))
                {
                    context.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    SOAPS.Ticket.CreateTicketResponse r = await client.CreateTicketAsync(new SOAPS.Ticket.CreateTicketRequest1(request));

                    return await DeserializeCustom<IdResponse>(r.CreateTicketResult);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GetTicketDetailsResponse> GetTicket(Guid id)
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            try
            {
                SOAPS.Ticket.IdRequest request = new SOAPS.Ticket.IdRequest
                {
                    Id = id.ToString()
                };
                SOAPS.Ticket.TicketSoapEndpointServiceClient client = new SOAPS.Ticket.TicketSoapEndpointServiceClient();

                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Bearer " + Token;

                OperationContext context = new OperationContext(client.InnerChannel);
                using (new OperationContextScope(context))
                {
                    context.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    SOAPS.Ticket.GetTicketDetailsResponse1 r = await client.GetTicketDetailsAsync(new SOAPS.Ticket.GetTicketDetailsRequest(request));

                    return await DeserializeCustom<GetTicketDetailsResponse>(r.GetTicketDetailsResult);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GetTicketDocumentResponse> GetTicketDocument(Guid id)
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            try
            {
                SOAPS.Ticket.IdRequest request = new SOAPS.Ticket.IdRequest
                {
                    Id = id.ToString()
                };
                SOAPS.Ticket.TicketSoapEndpointServiceClient client = new SOAPS.Ticket.TicketSoapEndpointServiceClient();

                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Bearer " + Token;

                OperationContext context = new OperationContext(client.InnerChannel);
                using (new OperationContextScope(context))
                {
                    context.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    SOAPS.Ticket.GetTicketDocumentResponse1 r = await client.GetTicketDocumentAsync(new SOAPS.Ticket.GetTicketDocumentRequest(request));

                    return await DeserializeCustom<GetTicketDocumentResponse>(r.GetTicketDocumentResult);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GetUserTicketsListResponse> GetCurrentUserTickets()
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            try
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Bearer " + Token;

                SOAPS.Ticket.TicketSoapEndpointServiceClient client = new SOAPS.Ticket.TicketSoapEndpointServiceClient();

                OperationContext context = new OperationContext(client.InnerChannel);
                using (new OperationContextScope(context))
                {
                    context.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    SOAPS.Ticket.GetCurrentUserTicketsListResponse r = await client.GetCurrentUserTicketsListAsync(new SOAPS.Ticket.GetCurrentUserTicketsListRequest());

                    return await DeserializeCustom<GetUserTicketsListResponse>(r.GetCurrentUserTicketsListResult);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task UpdateUser(UpdateUserRequest data)
        {
            if (!IsAuthenticated)
            {
                return;
            }

            try
            {
                SOAPS.User.UpdateUserRequest request = new SOAPS.User.UpdateUserRequest
                {
                    Id = data.Id.ToString(),
                    Address = data.Address,
                    Email = data.Email,
                    IsAdmin = data.IsAdmin,
                    Name = data.Name,
                    PhoneNumber = data.PhoneNumber,
                    Surname = data.Surname
                };
                SOAPS.User.UserSoapEndpointServiceClient client = new SOAPS.User.UserSoapEndpointServiceClient();

                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Bearer " + Token;

                OperationContext context = new OperationContext(client.InnerChannel);
                using (new OperationContextScope(context))
                {
                    context.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    SOAPS.User.UpdateUserResponse r = await client.UpdateUserAsync(new SOAPS.User.UpdateUserRequest1(request));
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task ChangePassword(ChangePasswordRequest data)
        {
            if (!IsAuthenticated)
            {
                return;
            }

            try
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Bearer " + Token;

                SOAPS.User.ChangePasswordRequest request = new SOAPS.User.ChangePasswordRequest
                {
                    UserId = data.UserId.ToString(),
                    NewPassword = data.NewPassword,
                    OldPassword = data.OldPassword
                };
                SOAPS.User.UserSoapEndpointServiceClient client = new SOAPS.User.UserSoapEndpointServiceClient();

                OperationContext context = new OperationContext(client.InnerChannel);
                using (new OperationContextScope(context))
                {
                    context.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    SOAPS.User.ChangePasswordResponse r = await client.ChangePasswordAsync(new SOAPS.User.ChangePasswordRequest1(request));
                }
            }
            catch (Exception)
            {

            }
        }

        public static async Task<T> DeserializeCustom<T>(object obj)
        {
            string json = obj.ToJson();
            Newtonsoft.Json.JsonSerializerSettings jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
            };
            jsonSerializerSettings.Converters.Add(new IsoTimeSpanConverter());

            T response = await json.ToObjectAsync<T>(jsonSerializerSettings);

            return response;
        }
    }
}
