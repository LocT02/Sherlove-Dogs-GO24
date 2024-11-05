using System;

namespace Result;
{
    public class Result<T>
    {
        // Attributes
        public bool isSuccess { get; private set; }

        public T value { get; private set; }

        public string message { get; private set; }

        public Exception error { get; private set; }

        // constructor
        private Result(bool IsSuccess, T Value, string Message, Exception Error)
        {
            isSuccess = IsSuccess;
            value = Value;
            message = Message;
            error = Error;
        }

        // Success method
        public static Result<T> Success(T value, string message = "Operation Successful", )
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
            if (isSuccess)
            {
                //log successes?
            } else
            {
                // log failure?
            }
        }

        // Get Value
        public T getValue(T defaultValue = default)
        {
            return isSuccess ? value : defaultValue;
        }

    }
}