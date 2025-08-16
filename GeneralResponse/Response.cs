namespace RakbnyMa_aak.GeneralResponse
{
    public class Response<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool IsSucceeded { get; set; }
        public int StatusCode { get; set; } = 200;

        public Response() { }

        public static Response<T> Success(T data, string message = null, int statusCode = 200)
            => new Response<T>
            {
                Data = data,
                IsSucceeded = true,
                Message = message ?? "تمت العملية بنجاح",
                StatusCode = statusCode
            };

        public static Response<T> Fail(string message, T data = default, int statusCode = 400)
            => new Response<T>
            {
                Data = data,
                IsSucceeded = false,
                Message = message ?? "فشلت العملية",
                StatusCode = statusCode
            };
    }
}
