using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Dashboard.Backend.Extensions;
using dotenv.net;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
var config = builder.Configuration;
builder.WebHost.ConfigureHosting(config);
builder.Services.ConfigureDependencies(config);
var app = builder.Build();
app.ConfigurePipeline(config);
app.Run();
