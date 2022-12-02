﻿namespace ShoppingLikeFlies.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplication AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSecurity(builder.Configuration);

        builder.Services.AddDefaultServices(builder.Configuration);

        builder.Services.AddCustomService(builder.Configuration);

        return builder.Build();
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Host.UseSerilog();
        return builder;
    }
}
