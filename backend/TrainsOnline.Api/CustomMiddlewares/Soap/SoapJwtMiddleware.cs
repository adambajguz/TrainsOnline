namespace TrainsOnline.Api.CustomMiddlewares.Soap
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.AspNetCore.Http;
    using SoapCore.Extensibility;
    using SoapCore.ServiceModel;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Common.Extensions;

    public class SoapJwtMiddleware : IServiceOperationTuner
    {
        private const string BearerAuth = "Bearer ";

        private IJwtService JwtService { get; }

        public SoapJwtMiddleware(IJwtService jwtService)
        {
            JwtService = jwtService;
        }

        public void Tune(HttpContext httpContext, object serviceInstance, OperationDescription operation)
        {
            // 1. Get validation attributes
            Type? operationDeclarationType = operation.DispatchMethod.DeclaringType;
            MethodInfo operationMethodInfo = operation.DispatchMethod;

            object[] soapAuthorizeAttributesInDT = operationDeclarationType?.GetCustomAttributes(typeof(SoapAuthorizeAttribute), true) ?? throw new NullReferenceException($"{nameof(operationDeclarationType)} is null");
            object[] soapAuthorizeClassAttributes = operationMethodInfo.GetCustomAttributes(typeof(SoapAuthorizeAttribute), true);

            SoapAuthorizeAttribute? attributeDT = soapAuthorizeAttributesInDT.FirstOrDefault() as SoapAuthorizeAttribute;
            SoapAuthorizeAttribute? attribute = soapAuthorizeClassAttributes.FirstOrDefault() as SoapAuthorizeAttribute;

            // 2. Check if operation is restricted by authorization
            if (attributeDT is null && attribute is null)
                return;

            // 3. Get and validate token
            Microsoft.Extensions.Primitives.StringValues token = httpContext.Request.Headers["Authorization"];

            if (token.Count != 1)
                throw new UnauthorizedAccessException("Invalid token");

            string tokenString = token.ToString();
            if (tokenString.Length <= BearerAuth.Length)
                throw new UnauthorizedAccessException("Invalid token");

            if (tokenString.StartsWith(BearerAuth))
                tokenString = tokenString.Remove(0, BearerAuth.Length);
            else
                throw new UnauthorizedAccessException("Invalid authorization");

            JwtService.ValidateStringToken(tokenString);

            // 4. Validate access to Service
            if (attributeDT != null)
            {
                string[] attributeDTRoles = attributeDT.Roles.RemoveWhitespaces()
                                                             .Split(',', StringSplitOptions.RemoveEmptyEntries);

                bool hasAccess = JwtService.IsAnyOfRolesInToken(tokenString, attributeDTRoles);
                if (!hasAccess)
                    throw new UnauthorizedAccessException("Invalid role");
            }

            // 5. Validate access to method
            if (attribute != null)
            {
                string[] attributeRoles = attribute!.Roles.RemoveWhitespaces()
                                                          .Split(',', StringSplitOptions.RemoveEmptyEntries);

                bool hasAccess = JwtService.IsAnyOfRolesInToken(tokenString, attributeRoles);
                if (!hasAccess)
                    throw new UnauthorizedAccessException("Invalid role");
            }

            //if (!JwtService.IsTokenStringValid(tokenString))
            //    throw new UnauthorizedAccessException("Invalid token");
        }
    }
}
