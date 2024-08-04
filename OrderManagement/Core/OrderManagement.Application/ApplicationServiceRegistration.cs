using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Caching;
using OrderManagement.Application.CQRS.Commands.CompanyCommands;
using OrderManagement.Application.CQRS.Validations.CompanyValidations;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Security.EmailAuthenticator;
using OrderManagement.Application.Security.JWT;
using OrderManagement.Application.Security.OtpAuthenticator.OtpNet;
using OrderManagement.Application.Security.OtpAuthenticator;
using System.Reflection;

namespace OrderManagement.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

        //services.AddTransient<IValidator<CreateCompanyCommand>, CreateCompanyValidator>();

        services.AddValidatorsFromAssemblyContaining<CreateCompanyValidator>();

        services.AddMemoryCache();
        services.AddScoped<ICachingService, MemoryCacheService>();
        services.AddScoped<ITokenHelper, JwtHelper>();
        services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();
        //    config.InvalidModelStateResponseFactory = context =>
        //    {

        //        var errors = context.ModelState.Values
        //        .Where(x => x.Errors.Any())
        //        .SelectMany(x => x.Errors)
        //        .Select(x => x.ErrorMessage).ToList();

        //        return new BadRequestObjectResult(errors)
        //        {
        //            StatusCode = StatusCodes.Status400BadRequest
        //        };
        //    };
        //});

        // .NET 8'e göre yorum satırlarını açabilirsiniz:
        // services.AddScoped<BrandBusinessRules>();
        // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheRemovingBehavior<,>));
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

        return services;
    }
}
