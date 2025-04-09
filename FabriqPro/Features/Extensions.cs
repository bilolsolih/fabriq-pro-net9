namespace FabriqPro.Features;

public static class Extensions
{
    public static string GetUploadsBaseUrl(this HttpContext httpContext)
    {
        var request = httpContext.Request;
        return $"{request.Scheme}://{request.Host}/uploads";
    }
}