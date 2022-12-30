using FediNet.Infrastructure;

public static class ResultExtensions
{
    public static IResult Xml<T>(this IResultExtensions _, T result, string? contentType = null, int? statusCode = null) =>
        new XmlResult<T>(result, contentType, statusCode);

    public static IResult JsonActivity<T>(this IResultExtensions _, T result, string? contentType = null, int? statusCode = null) =>
        Results.Json(result, contentType: contentType ?? "application/activity+json", statusCode: statusCode);
}
