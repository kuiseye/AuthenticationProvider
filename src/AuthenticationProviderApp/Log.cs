namespace AuthenticationProvider;
internal static partial class Log
{
    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to apply migrations to database")]
    public static partial void MigrationsFailed(ILogger logger, Exception exception);

    [LoggerMessage(Level = LogLevel.Error, Message = "Exception occured")]
    public static partial void InternalError(ILogger logger, Exception exception);
}