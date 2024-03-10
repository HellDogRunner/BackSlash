using System;

namespace RedMoonGames.Basics
{
    public struct TryResult
    {
        public readonly bool IsSuccessfully;
        public readonly Exception ResultException;

        public TryResult(bool isSuccessfully)
        {
            IsSuccessfully = isSuccessfully;
            ResultException = null;
        }

        public TryResult(Exception exception)
        {
            IsSuccessfully = exception == null;
            ResultException = exception;
        }

        public static TryResult Successfully
        {
            get => new TryResult(true);
        }

        public static TryResult Fail
        {
            get => new TryResult(false);
        }

        public static TryResult Exception(Exception exception)
        {
            return new TryResult(exception);
        }

        public static implicit operator bool (TryResult result)
        {
            return result.IsSuccessfully;
        }

        public static implicit operator TryResult(bool result)
        {
            return new TryResult(result);
        }
    }
}
