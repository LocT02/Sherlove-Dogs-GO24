
namespace IResultManager
{
    public interface IResult
    {
        bool IsSuccess { get; }
        string Error { get; }
        bool IsFailure { get; }
    }

    public interface IResult<T> : IResult
    {
        T Value { get; }
    }
}