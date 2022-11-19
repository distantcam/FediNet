namespace Microsoft.Extensions.Logging;

public static class LoggerExtensions
{
    public static void Trace<T>(this ILogger<T> logger, string? message, params object?[] args) => logger.LogTrace(message, args);
    public static void Trace<T>(this ILogger<T> logger, Exception? exception, string? message, params object?[] args) => logger.LogTrace(exception, message, args);
    public static void Debug<T>(this ILogger<T> logger, string? message, params object?[] args) => logger.LogDebug(message, args);
    public static void Debug<T>(this ILogger<T> logger, Exception? exception, string? message, params object?[] args) => logger.LogDebug(exception, message, args);
    public static void Info<T>(this ILogger<T> logger, string? message, params object?[] args) => logger.LogInformation(message, args);
    public static void Info<T>(this ILogger<T> logger, Exception? exception, string? message, params object?[] args) => logger.LogInformation(exception, message, args);
    public static void Warn<T>(this ILogger<T> logger, string? message, params object?[] args) => logger.LogWarning(message, args);
    public static void Warn<T>(this ILogger<T> logger, Exception? exception, string? message, params object?[] args) => logger.LogWarning(exception, message, args);
    public static void Error<T>(this ILogger<T> logger, string? message, params object?[] args) => logger.LogError(message, args);
    public static void Error<T>(this ILogger<T> logger, Exception? exception, string? message, params object?[] args) => logger.LogError(exception, message, args);
    public static void Fatal<T>(this ILogger<T> logger, string? message, params object?[] args) => logger.LogCritical(message, args);
    public static void Fatal<T>(this ILogger<T> logger, Exception? exception, string? message, params object?[] args) => logger.LogCritical(exception, message, args);
}
