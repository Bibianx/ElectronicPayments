namespace Models.Response
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Tipo { get; set; } = "info";
        public object Data { get; set; }
        public object DataBack { get; set; }
        public string Tabla { get; set; }
        public string Error { get; set; }
        public int TotalData { get; set; }
    }

    public class ServiceResponse<T> : BaseResponse
    {
        public new T Data { get; set; }
    }
}
