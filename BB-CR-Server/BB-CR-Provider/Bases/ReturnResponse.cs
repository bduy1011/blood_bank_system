using System.Net;

namespace BB.CR.Providers.Bases
{
    public record ReturnResponse<T>
    {
        public T? Data { get; set; }
        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; }
    }
}
