namespace Ali_Mav.BlogAPI.Models.Response
{
    public interface IBaseResponse<T>
    {
        T? Data { get; set; }
        string Description { get; set; }
        public bool success { get; set; }
    }
}
