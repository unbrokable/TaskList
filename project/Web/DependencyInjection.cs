using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddFluentValidationAutoValidation();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen( c =>
        {
            c.MapType<TimeSpan>(() => new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("00:00:00")
            });

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskList API", Version = "v1" });

            //Todo: remove after adding auth 
            c.AddDevAuthToSwagger();
        });

        return services;
    }

    private static void AddDevAuthToSwagger(this SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.AddSecurityDefinition("TestUserId", new OpenApiSecurityScheme
        {
            Name = "TestUserId",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Description = "Enter user id"
        });

        swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "TestUserId"
                        }
                    },
                    new string[] {}
                }
            });
    }
}
