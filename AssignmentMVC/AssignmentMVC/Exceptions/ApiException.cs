namespace AssignmentMVC.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public string Details { get; set; }

        public ApiException(string message, int statusCode, string details = "") : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }
    }
}
