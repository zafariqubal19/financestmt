using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using RSCS.FinancingStatements.Core.Services;
using RSCS.FinancingStatements.Data.Persistance.DataAccess;
using RSCS.FinancingStatements.Data.Repository;
using RSCS.FinancingStatements.Core.Interfaces.DataAccess;
using RSCS.FinancingStatements.Core.Interfaces.Repository;
using RSCS.FinancingStatements.Core.Interfaces.Service;
using Serilog;
using System.Data;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDbConnection>((s) => new SqlConnection(builder.Configuration.GetConnectionString("RSCSDatabase")));
builder.Services.AddScoped<IDBTransactionFactory, DBTransactionFactory>();
builder.Services.AddScoped<IDbTransaction>(s =>
{
    IDBTransactionFactory conn = s.GetRequiredService<IDBTransactionFactory>();
    return conn.Get();
});

// Register DAL and Unit Of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register all repositories 
builder.Services.AddScoped<ISettingRepository, SettingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
builder.Services.AddScoped<IFinProgramsRepository, FinProgramsRepository>();
builder.Services.AddScoped<IFinProgramsFranchiseeRepository, FinProgramsFranchiseeRepository>();
builder.Services.AddScoped<IInvoiceDetailsRepository, InvoiceDetailsRepository>();
builder.Services.AddScoped<IStatementDetailsRepository, StatementDetailsRepository>();
builder.Services.AddScoped<IReferencesRepository, ReferencesRepository>();
builder.Services.AddScoped<IContactsRepository, ContactsRepository>();
builder.Services.AddScoped<IResourceSecurityRepository, ResourceSecurityRepository>();

// Register all services
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFinProgramsService, FinProgramsService>();
builder.Services.AddScoped<IFinProgramsFranchiseeService, FinProgramsFranchiseeService>();
builder.Services.AddScoped<IInvoiceDetailsService, InvoiceDetailsService>();
builder.Services.AddScoped<IStatementDetailsService, StatementDetailsService>();
builder.Services.AddScoped<IReferencesService, ReferencesService>();
builder.Services.AddScoped<IContactsService, ContactsService>();
builder.Services.AddScoped<IResourceSecurityService, ResourceSecurityService>();

// Register Serilog
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT Bearer token support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RSCS.FinancingStatement.API", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
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
                        }
                    },
                    Array.Empty<string>()
                }
            });
});

// Authentication configuration using JWT and API Key
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };
    });

// Authorization configuration with API Key validation
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiKeyPolicy", policy =>
    {
        policy.RequireAssertion(context =>
        {
            var httpRequest = context.Resource as HttpRequest;
            if (httpRequest != null && httpRequest.Headers.ContainsKey("X-API-KEY"))
            {
                var apiKey = httpRequest.Headers["X-API-KEY"].ToString();
                return apiKey == builder.Configuration["ApiKey"];
            }
            return false;
        });
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
       builder => builder.AllowAnyOrigin()
                         .AllowAnyHeader()
                         .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    string virDirectory = builder.Configuration.GetSection("VirtualDirectory").Value;
    c.SwaggerEndpoint(virDirectory + "/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = virDirectory + "/Swagger";  // Set to root if you want Swagger at the root URL
});

app.UseCors("AllowAllOrigins");

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
