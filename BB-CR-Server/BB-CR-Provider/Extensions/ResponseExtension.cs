using BB.CR.Providers.Bases;
using System.Net;

namespace BB.CR.Providers.Extensions
{
    public static class ResponseExtension
    {
        public static void Success<T>(this ReturnResponse<T> response
            , T? data
            , string? message)
        {
            response.Data = data;
            response.Message = message;
            response.Status = HttpStatusCode.OK;
        }

        public static void Error<T>(this ReturnResponse<T> response
            , HttpStatusCode status
            , string? message)
        {
            response.Data = default;
            response.Message = message;
            response.Status = status;
        }
    }
}
