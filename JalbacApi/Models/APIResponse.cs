using System.Net;

namespace JalbacApi.Models
{
    public class APIResponse
    {
        public HttpStatusCode statusCode { get; set; }
        public bool IsExistoso { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Resultado { get; set; }
    }
}
