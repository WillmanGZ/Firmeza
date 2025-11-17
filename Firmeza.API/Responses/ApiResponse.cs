namespace Firmeza.API.Responses
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = String.Empty;
        public T? Payload { get; set; }
    }
}
