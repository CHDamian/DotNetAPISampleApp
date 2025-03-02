namespace DotNetAPISampleApp.Common
{
    public class Result
    {
        public bool Success { get; }
        public string? Message { get; }
        public List<string>? Errors { get; }

        // Konstruktor dla wyników bez danych
        protected Result(bool success, string? message = null, List<string>? errors = null)
        {
            Success = success;
            Message = message;
            Errors = errors;
        }

        public static Result Ok(string message = "Success") =>
            new Result(true, message);

        public static Result Fail(string message, List<string>? errors = null) =>
            new Result(false, message, errors);
    }

    public class Result<T> : Result
    {
        public T? Data { get; }
        protected Result(bool success, T? data, string? message = null, List<string>? errors = null)
            : base(success, message, errors)
        {
            Data = data;
        }

        public static Result<T> Ok(T data, string message = "Success") =>
            new Result<T>(true, data, message);

        public static new Result<T> Fail(string message, List<string>? errors = null) =>
            new Result<T>(false, default, message, errors);
    }


}
