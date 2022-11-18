using Dashboard.Backend.Facilities;
using Dashboard.Shared;
using Grpc.Core;
using static Dashboard.Shared.Reporter;

namespace Dashboard.Backend.GrpcServices;

public class ReporterService : ReporterBase
{
    private readonly EventCache eventCache;

    public ReporterService(EventCache eventCache)
    {
        this.eventCache = eventCache;
    }

    public override Task<ReportResponse> Report(ReportRequest request, ServerCallContext context)
    {
        var events = eventCache.GetEvents(request.Skip, request.Take);
        var response = new ReportResponse();
        response.Items.AddRange(events);
        return Task.FromResult(response);
    }
}