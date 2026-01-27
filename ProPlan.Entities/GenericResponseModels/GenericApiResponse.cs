namespace ProPlan.Entities.GenericResponseModels
{
    public class GenericApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public int? StatusCode { get; set; }

        public static GenericApiResponse<T> SuccessResponse(T data, string? message = null, int? statusCode = null)
            => new()
            {
                Success = true,
                Data = data,
                Message = message ?? "İşlem başarıyla tamamlandı.",
                StatusCode = statusCode ?? 200
            };

        public static GenericApiResponse<T> FailResponse(string message, List<string>? errors = null, int? statusCode = null)
            => new()
            {
                Success = false,
                Message = message,
                Errors = errors ?? new(),
                StatusCode = statusCode ?? 400
            };

        public static GenericApiResponse<T> NotFoundResponse(string message = "Kaynak bulunamadı.")
            => new()
            {
                Success = false,
                Message = message,
                StatusCode = 404
            };

        public static GenericApiResponse<T> UnauthorizedResponse(string message = "Yetkiniz bulunmamaktadır.")
            => new()
            {
                Success = false,
                Message = message,
                StatusCode = 401
            };
    }
}
