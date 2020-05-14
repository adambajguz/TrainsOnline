namespace TrainsOnline.Api.CustomMiddlewares.Soap
{
    using System;

    //
    // Summary:
    //     Specifies that the interface or method that this attribute is applied to requires
    //     the specified authorization.
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SoapAuthorizeAttribute : Attribute
    {
        //
        // Summary:
        //     Gets or sets a comma delimited list of roles that are allowed to access the resource.
        public string Roles { get; set; } = string.Empty;

        public SoapAuthorizeAttribute()
        {

        }
    }
}
