using System;

namespace Rogue
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }

        public T Value { get; set; }

        public static Result<T> Success => new Result<T>() { IsSuccess = true };

        public static Result<T> Failure => new Result<T>() { };

        public Result<T> Set(T value)
        {
            this.Value = value;
            return this;
        }

        public static bool operator ==(Result<T> a, Result<T> b)
        {
            return a.IsSuccess == b.IsSuccess
                || Object.ReferenceEquals(a.Value, b.Value);
        }

        public static bool operator !=(Result<T> a, Result<T> b)
        {
            return a.IsSuccess != b.IsSuccess
                || !Object.ReferenceEquals(a.Value, b.Value);
        }

        public static implicit operator bool(Result<T> a) => a.IsSuccess;
    }
}