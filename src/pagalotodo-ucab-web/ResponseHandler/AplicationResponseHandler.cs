using System.Net;

namespace UCABPagaloTodoWeb.ResponseHandler
{
    public class AplicationResponseHandler<T> where T : class
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Exception { get; set; }
    }
}