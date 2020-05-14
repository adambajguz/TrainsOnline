namespace TrainsOnline.Desktop.Domain.Extensions
{
    using RestSharp;

    public static class RestRequestExtensions
    {
        public static RestRequest AddBearerAuthentication(this RestRequest request, string token)
        {
            request.AddParameter("Authorization", $"Bearer {token}", ParameterType.HttpHeader);

            return request;
        }

        public static IRestRequest AddBearerAuthentication(this IRestRequest request, string token)
        {
            request.AddParameter("Authorization", $"Bearer {token}", ParameterType.HttpHeader);

            return request;
        }
    }
}
