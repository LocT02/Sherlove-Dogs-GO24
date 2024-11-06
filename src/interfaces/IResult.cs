using System;

namespace IResult
{
    public interface IResult<T> { // interface for result 
        bool IsSuccess { get; }
        T Value { get; }
        Exception Error { get; }
        void Logger();
    }
}