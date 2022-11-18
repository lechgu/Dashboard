using System.Globalization;
using Dashboard.Frontend.Facilities;
using Dashboard.Shared;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;

namespace Frontend;

public partial class App
{
    HxGrid<Event> grid;

    [Inject]
    ReportFacility ReportFacility { get; set; }

    [Inject]
    NotificationFacility NotificationFacility { get; set; }

    protected override async Task OnInitializedAsync()
    {
        NotificationFacility.NewEvent += async () => await grid.RefreshDataAsync();
        await NotificationFacility.ConnectAsync();
    }

    async Task<GridDataProviderResult<Event>> GetGridData(GridDataProviderRequest<Event> request)
    {
        var events = await ReportFacility.GetEventsAsync(0, 10);
        return new GridDataProviderResult<Event>
        {
            Data = events,
            TotalCount = events.Count()
        };
    }
}