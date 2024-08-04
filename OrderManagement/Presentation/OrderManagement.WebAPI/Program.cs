using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using OrderManagement.Application;
using OrderManagement.Application.Security.JWT;
using OrderManagement.Persistence.Extensions;
using OrderManagement.WebAPI.Middleware;
using System.Text;
var logger = NLogBuilder.ConfigureNLog("nLog.config").GetCurrentClassLogger();

try
{
    logger.Debug("init main");
    var builder = WebApplication.CreateBuilder(args);
    //var configuration = builder.Configuration;
    //var tokenOptions = configuration.GetSection("TokenOptions").Get<OrderManagement.Application.Security.JWT.TokenOptions>();

    //var key = Encoding.UTF8.GetBytes(tokenOptions.SecurityKey);
    //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //    .AddJwtBearer(options =>
    //    {
    //        options.TokenValidationParameters = new TokenValidationParameters
    //        {
    //            ValidateIssuer = true,
    //            ValidateAudience = true,
    //            ValidateLifetime = true,
    //            ValidateIssuerSigningKey = true,
    //            ValidIssuer = tokenOptions.Issuer,
    //            ValidAudience = tokenOptions.Audience,
    //            IssuerSigningKey = new SymmetricSecurityKey(key)
    //        };

    //        options.Events = new JwtBearerEvents
    //        {
    //            OnAuthenticationFailed = context =>
    //            {
    //                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
    //                {
    //                    context.Response.Headers.Add("Token-Expired", "true");
    //                }
    //                return Task.CompletedTask;
    //            }
    //        };
    //    });

    var configuration = builder.Configuration;
    var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();

    if (tokenOptions == null)
    {
        throw new InvalidOperationException("TokenOptions configuration section is missing or invalid.");
    }

    builder.Services.AddSingleton<ITokenHelper, JwtHelper>();
    builder.Services.AddSingleton(tokenOptions);


    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(LogLevel.Trace);
    builder.Host.UseNLog();

    builder.Services.AddApplicationServices();
    builder.Services.Program();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    var key = Encoding.UTF8.GetBytes(tokenOptions.SecurityKey);

    builder.Logging.ClearProviders(); // Varsayýlan günlük saðlayýcýlarýný temizleyin
    builder.Logging.AddConsole(); // Konsola günlükleme ekleyin
    builder.Logging.AddDebug(); // Debug'a günlükleme ekleyin

    // JWT yapýlandýrmasý
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["TokenOptions:Issuer"],
        ValidAudience = builder.Configuration["TokenOptions:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenOptions:SecurityKey"])),
        //ClockSkew = TimeSpan.Zero // Token süresi ile ilgili hatalarýn önüne geçmek için
    };

    //options.Events = new JwtBearerEvents
    //{
    //    OnAuthenticationFailed = context =>
    //    {
    //        // Yanýt kodunu deðiþtirmeden önce yanýtý baþlatmamalýsýnýz
    //        context.NoResult(); // Yanýt baþlatmýyor, sadece iþleme devam etme
    //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //        context.Response.ContentType = "text/plain";
    //        return context.Response.WriteAsync("Authentication failed.");
    //    },
    //    OnChallenge = context =>
    //    {
    //        // Yanýt kodunu deðiþtirmeden önce yanýtý baþlatmamalýsýnýz
    //        context.HandleResponse();
    //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //        context.Response.ContentType = "text/plain";
    //        return context.Response.WriteAsync("Authentication required.");
    //    }
    //};
});
    builder.Services.AddAuthorization();
    // Swagger ayarlarý
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT token with Bearer into field \n Example: 'Bearer your_token_here'",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
            },
            Array.Empty<string>()
        }
    });
});


    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.ConfigureExceptionMiddleware();
    app.UseHttpsRedirection();

    // app.UseExceptionHandler();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();



    app.MapControllers();

    app.Run();
}

catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
