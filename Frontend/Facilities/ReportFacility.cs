using System.Net.Http;
using Dashboard.Shared;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using static Dashboard.Shared.Reporter;

namespace Dashboard.Frontend.Facilities;

public class ReportFacility
{
    public async Task<IEnumerable<Event>> GetEventsAsync(int skip, int take)
    {
        var channel = GrpcChannel.ForAddress("https://localhost:5003", new GrpcChannelOptions
        {
            HttpHandler = new GrpcWebHandler(new HttpClientHandler())
        });
        var client = new ReporterClient(channel);
        var request = new ReportRequest
        {
            Skip = skip,
            Take = take
        };
        var response = await client.ReportAsync(request);
        return response.Items;
    }
}