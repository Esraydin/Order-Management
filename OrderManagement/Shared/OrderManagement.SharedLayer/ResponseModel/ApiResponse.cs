using OrderManagement.SharedLayer.Enums;

namespace OrderManagement.SharedLayer.ResponseModel
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public ErrorDto ErrorDto { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }
        public ResponseType ResponseType { get; set; }

        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T> { Data = data, IsSuccess = true, ResponseType = ResponseType.Success };
        }

        public static ApiResponse<T> Success(ResponseType responseType)
        {
            return new ApiResponse<T> { Data = default, ResponseType = responseType, IsSuccess = true };
        }

        public static ApiResponse<T> Fail(string error, ResponseType responseType)
        {
            ErrorDto errorDto = new ErrorDto(error, true);
            return new ApiResponse<T> { ErrorDto = errorDto, ResponseType = responseType, IsSuccess = false };
        }

        public static ApiResponse<T> Fail(ErrorDto errorDto, ResponseType responseType)
        {
            return new ApiResponse<T> { ErrorDto = errorDto, ResponseType = responseType, IsSuccess = false };
        }

        public static ApiResponse<T> ValidationError(List<ValidationError> errors, ResponseType responseType)
        {
            return new ApiResponse<T> { ValidationErrors = errors, ResponseType = responseType, IsSuccess = false };
        }
    }
}
