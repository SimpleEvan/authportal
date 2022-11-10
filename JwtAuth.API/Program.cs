using Auth.Storage.Dependency;
using JwtAuth.API.Dependency;
using JwtAuth.API.Services;
using JwtAuth.API.Services.Interfaces;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.AddKeyVaultDependency();

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDependency(builder.Configuration);
builder.Services.AddCosmosDbDependency(builder.Configuration);

builder.Services.AddScoped<IAuthPortalService, AuthPortalService>();

builder.Services.Configure<AuthPortalServiceOptions>(options =>
    options.IssuerSecretKey = builder.Configuration.GetSection("AuthPortalIssuerSecretKey").Value ?? string.Empty);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
    
