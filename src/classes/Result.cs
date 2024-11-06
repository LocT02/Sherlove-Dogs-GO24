using System;

namespace Result
{
    public class Result<T>
    {
        // Attributes
        public bool IsSuccess { get; private set; }

        public T Value { get; private set; }

        public string Message { get; private set; }

        public Exception Error { get; private set; }

        // constructor
        private Result(bool isSuccess, T value, string message, Exception error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Message = message;
            Error = error;
        }

        // Success method
        public static Result<T> Success(T value, string message = "Operation Successful")
        {
            return new Result<T>(true, value, message, null);
        }

        // Failed
        public static Result<T> Failure(string message, Exception error = null)
        {
            return new Result<T>(false, default, message, error);
        }

        // Logger maybe?
        public void Logger()
        {
            // shrug dunno
            if (IsSuccess)
            {
                //log successes?
            } else
            {
                // log failure?
            }
        }
    }
}