namespace FlimesAPI.Middleware;

    public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 400;


            var response = new { message = ex.Message };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

