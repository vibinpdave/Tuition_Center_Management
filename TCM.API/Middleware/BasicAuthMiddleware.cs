namespace TCM.API.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IMediator mediator)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing Authorization header");
                return;
            }

            if (!AuthenticationHeaderValue.TryParse(authHeader, out var headerValue) ||
                !headerValue.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid Authorization scheme");
                return;
            }

            var credentialBytes = Convert.FromBase64String(headerValue.Parameter ?? "");
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

            if (credentials.Length != 2)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid credentials format");
                return;
            }

            var username = credentials[0];
            var password = credentials[1];

            // Call CQRS command in Application layer
            var command = new UserLoginCommand(username, password);

            try
            {
                var result = await mediator.Send(command);

                // Populate claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.objResponse.Id.ToString()),
                    new Claim(ClaimTypes.Name, result.objResponse.Name),
                    new Claim(ClaimTypes.Email, result.objResponse.Email),
                    new Claim("role_id", result.objResponse.UserRoleId.ToString())
                };

                var identity = new ClaimsIdentity(claims, "Basic");
                context.User = new ClaimsPrincipal(identity);

                // Return JSON response directly from middleware (optional)
                //context.Response.ContentType = "application/json";
                //await context.Response.WriteAsJsonAsync(result);
            }
            catch (UnauthorizedAccessException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid username or password");
            }

            // Optionally set context.User claims here for downstream
            await _next(context);
        }
    }
}
