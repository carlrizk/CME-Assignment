namespace Assignment_2_3_CarlRizk.Models
{
    public class ApiResponse<T>
    {
        public bool status { get; set; }
        public T data;

        public ApiResponse(bool status, T data)
        {
            this.status = status;
            this.data = data;
        }
    }
}
