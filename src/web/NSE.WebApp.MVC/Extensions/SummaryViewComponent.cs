using Microsoft.AspNetCore.Mvc;

namespace NSE.WebApp.MVC.Extensions;

public class SummaryViewComponent : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult<IViewComponentResult>(View());
    }
}