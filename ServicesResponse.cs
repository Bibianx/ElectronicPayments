namespace Models.Response
{
    public class BaseResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string tipo { get; set; } = "info";
        public object data { get; set; }
        public object dataBack { get; set; }
        public string tabla { get; set; }
        public string error { get; set; }
        public int totalData { get; set; }
    }

    public class ServiceResponse<T> : BaseResponse
    {
        public new T data { get; set; }
    }
}
