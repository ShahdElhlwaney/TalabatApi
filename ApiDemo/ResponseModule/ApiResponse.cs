namespace ApiDemo.ResponseModule
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode,string message=null)
        {
            StatusCode=statusCode;
            Message=message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        public int StatusCode {  get; set; }
        public string Message { get; set; }
        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Ok,Requset made Successfully",
                404 => "User Not Found",
                400=>"400Exception"
            };
        }
    }
}
