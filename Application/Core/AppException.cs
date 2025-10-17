namespace Application.Core
{
    public class AppException
    {
        public AppException(int statusCode, string message, object? details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        //public string? Details { get; set; }
        public object? Details { get; set; }
    }
}