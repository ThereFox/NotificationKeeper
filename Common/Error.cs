using CSharpFunctionalExtensions;

namespace Common
{
    public record Error
    (
        string ErrorMessage
    );

    public static class FromResult
    {
        public static Error AsError(this Result from)
        {
            if (from.IsSuccess)
            {
                throw new InvalidCastException("from dont contain error");
            }

            return new Error(from.Error);
        }
        public static Error AsError<T>(this Result<T> from)
        {
            if (from.IsSuccess)
            {
                throw new InvalidCastException("from dont contain error");
            }

            return new Error(from.Error);
        }
    }

}
