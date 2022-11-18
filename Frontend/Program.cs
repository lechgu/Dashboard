using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend;
using Havit.Blazor.Components.Web;
using Dashboard.Frontend.Facilities;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.Services.AddHxServices();
builder.Services.AddScoped<ReportFacility>();
builder.Services.AddScoped<NotificationFacility>();

await builder.Build().RunAsync();
