using System;
using IResultManager;

namespace ResultManager
{
    public class Result : IResult
    {
        //Result pattern class, will aid in returning errors successes, and objects

        public bool IsSuccess { get; }
        public string Error { get; private set; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool success, string error)
        {
            if (success && error != string.Empty)
                throw new InvalidOperationException();
            if (!success && error == string.Empty)
                throw new InvalidOperationException();
            IsSuccess = success;
            Error = error;
        }

        public static Result Failure(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Failure<T>(string message)
        {
            return new Result<T>(default, false, message);
        }

        public static Result Success()
        {
            return new Result(true, string.Empty);
        }

        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

    }

    public class Result<T> : Result, IResult<T>
    {
        public T Value { get; set; }

        protected internal Result(T value, bool success, string error) : base(success, error)
        {
            Value = value;
        }
    }
}