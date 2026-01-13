namespace BB.CR.Providers.Extensions
{
    public static class ExceptionExtension
    {
        public static string? InnerMessage(this System.Exception exception)
        {
            if (exception == null)
                return null;

            var inner = exception;
            while (inner?.InnerException is not null)
            {
                inner = inner?.InnerException;
            }

            return inner?.Message;
        }
    }
}
