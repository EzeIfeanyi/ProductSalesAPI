using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductSalesAPI.Application.ApiUtilities.Interfaces;
using ProductSalesAPI.Application.ApiUtilities.Services;
using ProductSalesAPI.Application.ApiUtilities.Shared;
using ProductSalesAPI.Application.Products.Command.CreateProduct;
using ProductSalesAPI.Application.Products.Query.RetrieveProducts;
using ProductSalesAPI.Application.SalesOrders.Command.CreateSalesOrder;
using ProductSalesAPI.Application.SalesOrders.Command.DeleteSalesOrder;
using ProductSalesAPI.Application.SalesOrders.Query.RetrieveSalesOrders;
using ProductSalesAPI.Application.UserAuthentication.Command.Login;
using ProductSalesAPI.Application.UserAuthentication.Command.Register;
using ProductSalesAPI.Infrastructure;
using ProductSalesAPI.Infrastructure.DataAccess;
using ProductSalesAPI.Infrastructure.DataAccess.Repositories;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ProductSalesAPI.Presentation.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure JwtSettings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Register database services
        services.AddScoped<IDatabaseContext, DatabaseContext>();
        services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

        // Other services
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISalesOrderService, SalesOrderService>();
        services.AddScoped<IUserService, UserService>();

        // Add MediatR handlers
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CreateProductCommandHandler>();
            cfg.RegisterServicesFromAssemblyContaining<CreateSalesOrderCommandHandler>();
            cfg.RegisterServicesFromAssemblyContaining<DeleteSalesOrderCommandHandler>();
            cfg.RegisterServicesFromAssemblyContaining<LoginUserCommandHandler>();
            cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommandHandler>();
            cfg.RegisterServicesFromAssemblyContaining<GetProductsQueryHandler>();
            cfg.RegisterServicesFromAssemblyContaining<GetSalesOrdersQueryHandler>();
        });

        // Configure JWT Authentication
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";
                    var response = new ApiResponse<string>(false, "Unauthorized access", "Access denied.");
                    return context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            };
        });

        // Add CORS policy
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        // Add SignalR
        services.AddSignalR();

        // Add Controllers
        services.AddControllers();
        services.AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<Program>();
        });

        // Add Swagger with JWT Bearer support
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        return services;
    }
}

