using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Dashboard.Shared;
using static Dashboard.Shared.Collector;
using System.Net.Http;

var channel = GrpcChannel.ForAddress("https://localhost:5003", new GrpcChannelOptions
{
    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
});
var client = new CollectorClient(channel);
var rawEvent = new RawEvent
{
    Category = "Program Editor",
    Severity = "Information",
    Description = "something happened"
};
await client.PublishAsync(rawEvent);
