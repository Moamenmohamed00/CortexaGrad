using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Cortexa.Api.Filters
{
    /// <summary>
    /// Action filter that measures and logs the execution time of API requests.
    /// Requests exceeding the warning threshold are logged at Warning level.
    /// </summary>
    public class PerformanceLoggingFilter : IAsyncActionFilter
    {
        private readonly ILogger<PerformanceLoggingFilter> _logger;

        /// <summary>
        /// Requests taking longer than this threshold (in milliseconds) are logged as warnings.
        /// </summary>
        private const long WarningThresholdMs = 500;

        public PerformanceLoggingFilter(ILogger<PerformanceLoggingFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionName = $"{context.Controller.GetType().Name}.{context.ActionDescriptor.DisplayName}";
            var httpMethod = context.HttpContext.Request.Method;
            var path = context.HttpContext.Request.Path;

            var stopwatch = Stopwatch.StartNew();

            var executedContext = await next();

            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;

            if (executedContext.Exception != null)
            {
                _logger.LogWarning(
                    "Action {Action} [{Method}] {Path} failed after {ElapsedMs}ms",
                    actionName, httpMethod, path, elapsedMs);
            }
            else if (elapsedMs > WarningThresholdMs)
            {
                _logger.LogWarning(
                    "SLOW: Action {Action} [{Method}] {Path} completed in {ElapsedMs}ms (threshold: {Threshold}ms)",
                    actionName, httpMethod, path, elapsedMs, WarningThresholdMs);
            }
            else
            {
                _logger.LogInformation(
                    "Action {Action} [{Method}] {Path} completed in {ElapsedMs}ms",
                    actionName, httpMethod, path, elapsedMs);
            }
        }
    }
}
