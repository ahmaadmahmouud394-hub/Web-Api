using Applicazione_1.Services;
using System.IO;
using System.Text.Json;

namespace Applicazione_1.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        public RequestLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower().ToString();
            var method = context.Request.Method.ToLower().ToString();
            Dictionary<string, List<string>> Authorizations = new Dictionary<string, List<string>>();
            Authorizations.Add("/api/test/get", new List<string>{"admin", "employee"});


            if (path != null && path.StartsWith("/api/test/login") && method == "post")
            {
                await _next(context);
                return;
            }

            else
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    JWT jwt = scope.ServiceProvider.GetRequiredService<JWT>();

                    string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                    var Role = jwt.ValidateAndGetUsername(token);

                    if (Authorizations.GetValueOrDefault(path ?? "mniunj8iu9njiu9")?.Contains(Role ?? "tesgsdgs") == true)
                    {
                        await _next(context);
                    }
                    else
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = "Unauthorized" }));
                    }

                }
            }
        }
    }
}
