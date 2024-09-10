namespace TaskList.Web.Extention;

public static class HttpContextExtensions
{
    public static string GetUserId(this HttpContext httpContext)
    {
        // Todo: remove after adding auth 
        if (httpContext.Request.Headers.TryGetValue("TestUserId", out var userId))
        {
            return userId.ToString();
        }

        throw new ArgumentException("The 'TestUserId' header is missing or empty.");
    }
}
