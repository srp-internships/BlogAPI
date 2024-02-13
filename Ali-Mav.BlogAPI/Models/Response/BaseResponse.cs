namespace Ali_Mav.BlogAPI.Models.Response
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public T? Data { get; set; }
        public string Description { get; set; }
        public bool success { get; set; } = false;
    }
}
