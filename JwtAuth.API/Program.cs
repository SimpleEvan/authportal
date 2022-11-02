using Auth.Storage.Dependency;
using JwtAuth.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settings = builder.Configuration.GetSection("CosmosDbSettings").Get<CosmosDbSettings>();
builder.Services.AddCosmosDbDependency(settings);

builder.Services.AddSingleton<IAuthService, AuthService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

