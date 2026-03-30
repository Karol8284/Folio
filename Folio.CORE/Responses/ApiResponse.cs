namespace Folio.CORE.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse() { }

        public ApiResponse(T data, string message = "Success")
        {
            Success = true;
            Data = data;
            Message = message;
        }

        public ApiResponse(string message)
        {
            Success = false;
            Message = message;
            Errors = new List<string> { message };
        }

        public ApiResponse(List<string> errors, string message = "Validation failed")
        {
            Success = false;
            Message = message;
            Errors = errors;
        }
    }
}
