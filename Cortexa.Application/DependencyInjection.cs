using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Reflection;

namespace Cortexa.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(assembly);
            });

            services.AddValidatorsFromAssembly(assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Common.Behaviors.ValidationBehavior<,>));

            return services;
        }
    }

    //public class LoggingBehavior<TRequest, TResponse>
    //: IPipelineBehavior<TRequest, TResponse>
    //{
    //    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    //    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    //    {
    //        _logger = logger;
    //    }

    //    public async Task<TResponse> Handle(
    //        TRequest request,
    //        RequestHandlerDelegate<TResponse> next,
    //        CancellationToken cancellationToken)
    //    {
    //        _logger.LogInformation("Path : {Path}",request.);
    //        return await next();
    //    }
    //}
}
