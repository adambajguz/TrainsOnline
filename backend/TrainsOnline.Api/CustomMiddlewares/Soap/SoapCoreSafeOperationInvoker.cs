namespace TrainsOnline.Api.CustomMiddlewares.Soap
{
    using System.Reflection;
    using System.Threading.Tasks;
    using Serilog;
    using SoapCore.Extensibility;

    public class SoapCoreSafeOperationInvoker : IOperationInvoker
    {
        public async Task<object?> InvokeAsync(MethodInfo methodInfo, object serviceInstance, object[] arguments)
        {
            Log.Information("SoapCoreSafeOperationInvoker: Invoke method {method} from {declearingType} with arguments {argumentsTypes} and service instance {serviceInstance}", methodInfo?.Name, methodInfo?.DeclaringType?.FullName, arguments, serviceInstance);

            // Invoke Operation method
            object? responseObject = methodInfo?.Invoke(serviceInstance, arguments);
            if ((methodInfo?.ReturnType.IsConstructedGenericType ?? false) && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                Task? responseTask = (Task?)responseObject;
                if (!(responseTask is null))
                {
                    await responseTask;
                    responseObject = responseTask?.GetType()
                                                 ?.GetProperty("Result")
                                                 ?.GetValue(responseTask);
                }
            }
            else if (responseObject is Task responseTask)
            {
                await responseTask;

                //VoidTaskResult
                responseObject = null;
            }

            Log.Information("SoapCoreSafeOperationInvoker: Return response {responseTypes}", responseObject);

            return responseObject;
        }
    }
}
