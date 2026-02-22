namespace Cortexa.Api.Middlewares
{
    /// <summary>
    /// Middleware that secures specific endpoints by validating an API Key
    /// passed in the "X-Api-Key" request header against the configured value.
    /// Endpoints decorated with [AllowAnonymous] or specific paths can be excluded.
    /// </summary>
    public class ApiKeyMiddleware
    {
        private const string ApiKeyHeaderName = "X-Api-Key";
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Paths that bypass API key validation (case-insensitive, prefix match).
        /// </summary>
        private static readonly string[] ExcludedPrefixes =
        [
            "/swagger",
            "/hubs/"
        ];

        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;

            // Skip validation for excluded paths
            if (ExcludedPrefixes.Any(prefix => path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            // If no API key is configured, skip validation entirely
            var configuredApiKey = _configuration["ApiKey"];
            if (string.IsNullOrEmpty(configuredApiKey))
            {
                await _next(context);
                return;
            }

            // Validate the header
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedApiKey))
            {
                _logger.LogWarning("API Key missing from request to {Path}", path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "API Key is required." });
                return;
            }

            if (!string.Equals(configuredApiKey, providedApiKey, StringComparison.Ordinal))
            {
                _logger.LogWarning("Invalid API Key provided for request to {Path}", path);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { message = "Invalid API Key." });
                return;
            }

            await _next(context);
        }
    }
}
