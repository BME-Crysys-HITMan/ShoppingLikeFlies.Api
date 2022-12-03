﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShoppingLikeFlies.Api.Configuration;
using ShoppingLikeFlies.Api.Security;
using ShoppingLikeFlies.Api.Security.DAL;
using ShoppingLikeFlies.Api.Services;
using System.Text;

namespace ShoppingLikeFlies.Api.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var secConfig = new SecurityConfiguration();

        configuration.Bind(SecurityConfiguration.SectionName, secConfig);

        services.Configure<SecurityConfiguration>(x =>
        {
            x.Issuer = secConfig.Issuer;
            x.Audience = secConfig.Audience;
            x.Duration = secConfig.Duration;
            x.Key = secConfig.Key;
        });

        services.AddDbContext<SecurityDbContext>(options =>
            options.UseSqlServer(secConfig.ConnectionString));

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<SecurityDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(o =>
        {
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = secConfig.Issuer,
                    ValidAudience = secConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secConfig.Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ValidToken", policy =>
                policy.Requirements.Add(new SecRequirement()));

            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));
        });

        services.AddSingleton<ITokenCache, TokenCache>();
        services.AddTransient<ITokenGenerator, TokenGenerator>();
        services.AddTransient<IAuthorizationHandler, SecRequirementHandler>();

        return services;
    }

    public static IServiceCollection AddDefaultServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    public static IServiceCollection AddCustomService(this IServiceCollection services, IConfiguration configuration)
    {
        string validatorPath = configuration.GetSection("CaffValidator").GetValue<string>("ValidatorPath");
        string GeneratorDir = configuration.GetSection("CaffValidator").GetValue<string>("GeneratorDir");
        bool useAzure = configuration.GetSection("CaffValidator").GetValue<bool>("UseAzure");
        string DirectoryPath = configuration.GetSection("CaffValidator").GetValue<string>("DirectoryPath");

        services.AddCaffProcessor(
            x => { x.Validator = validatorPath; },
            upload => { upload.ShouldUploadToAzure = useAzure; upload.DirectoryPath = DirectoryPath; },
            z => { z.GeneratorDir = GeneratorDir; },
            configuration);

        return services;
    }
}