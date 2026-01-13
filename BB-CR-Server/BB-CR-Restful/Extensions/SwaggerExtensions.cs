using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BB.CR.Rest.Extensions
{
    public class CustomHeaderGen : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters =
            [
                new OpenApiParameter
                {
                    Name = "FireBaseToken",
                    In = ParameterLocation.Header,
                    Description = "FCM token to receive notification newest version",
                },
                new OpenApiParameter
                {
                    Name = "AppVersion",
                    In = ParameterLocation.Header,
                    Description = "Version newest application",
                },
                new OpenApiParameter
                {
                    Name = "DeviceId",
                    In = ParameterLocation.Header,
                    Description = "Device identity of mobile",
                },
            ];
        }
    }
}
