using System.Net;

namespace Viamatica.Blog.Api.Modelos
{
    public class Response
    {
        public Response()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; }
        public List<string>? ErrorMessages { get; set; }
        public object? Result { get; set; }
    }
}
