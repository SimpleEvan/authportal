using Auth.Storage.Dependency;
using JwtAuth.API.Dependency;
using JwtAuth.API.Services;
using JwtAuth.API.Services.Interfaces;
using Serilog;
using Serilog.Events;
using System.Diagnostics.CodeAnalysis;


Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
try
{
    Log.Information("The webHost is starting");
    var builder = WebApplication.CreateBuilder(args);

    builder.AddKeyVaultDependency();

    SerilogSettings.AddApplicationInsightConfiguration(builder.Configuration);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddControllers();
    builder.Services.AddRouting(options => options.LowercaseUrls = true);
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerDependency(builder.Configuration);
    builder.Services.AddCosmosDbDependency(builder.Configuration);

    builder.Services.AddScoped<IAuthPortalService, AuthPortalService>();

    builder.Services.Configure<AuthPortalServiceOptions>(options =>
        options.IssuerSecretKey = builder.Configuration.GetSection("AuthPortalIssuerSecretKey").Value ?? string.Empty);

    builder.Services.AddCors(options => options.AddPolicy(name: "localTesting", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));

    var app = builder.Build();

    app.UseSerilogRequestLogging(configure =>   
    {
        configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms";
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors("localTesting");
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.MapGet("/api/health", () => $"health check is OK: {DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss 'GMT'")}");

    app.Run();
}
catch (Exception ex) 
{
    Log.Fatal($"Fatal startup error: {ex.Message}");
}
finally    
{
    Log.CloseAndFlush();
}


[ExcludeFromCodeCoverage]
public partial class Program { }