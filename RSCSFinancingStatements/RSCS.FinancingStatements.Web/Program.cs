using Microsoft.AspNetCore.Authentication.Negotiate;
using RSCS.FinancingStatements.Web.Facade;
using RSCS.FinancingStatements.Web.HttpHelper;
using RSCS.FinancingStatements.Web.Middleware;
using Serilog;
using SmartBreadcrumbs.Extensions;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using RSCS.FinancingStatements.Web.Common;
using System.Configuration;
using Rscs.SecureApi.Core.Client.Interfaces;
using Rscs.SecureApi.Core.Client.Services;

var builder = WebApplication.CreateBuilder(args);

// Add framework services.
builder.Services
	.AddControllersWithViews();
	builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
//SmartbreadCrumb Service
builder.Services.AddBreadcrumbs(Assembly.GetExecutingAssembly(), options =>
{
    options.TagName = "nav";
    options.TagClasses = "";
    options.OlClasses = "breadcrumb";
    options.LiClasses = "breadcrumb-item";
    options.ActiveLiClasses = "breadcrumb-item active";

	
});
// Add Kendo UI services to the services container
builder.Services.AddKendo();

// Add services to the container.
builder.Services.AddSingleton<IApplicationFacade, ApplicationFacade>();
builder.Services.AddSingleton<IHttpApiClient, HttpApiClient>();
builder.Services.AddSingleton<IRSCSAuthService, RSCSAuthService>();


//Register Serilog
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		.AddCookie();  //added cookies authentication while implementing logout option
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
	// By default, all incoming requests will be authorized according to the default policy.
	options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages();
builder.Services.AddMemoryCache(); // To Configure IMemoryCaching
var app = builder.Build();

//Configure the HTTP URLFactory APIbase url.
var configuration = app.Services.GetRequiredService<IConfiguration>();
RSCSAPIUrlFactory.SetConfiguration(configuration);

//Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseGlobalExceptionHandlerMiddleware();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.UseAuthorizationMiddleware();

app.MapControllerRoute(
	name: "default",
    pattern: "{controller=Invoice}/{action=GetFinPrograms}/{id?}");
app.Run();
