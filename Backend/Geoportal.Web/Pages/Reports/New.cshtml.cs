using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Geoportal.Web.Pages.Reports;

public class NewModel : PageModel
{
    public bool ShowSuccess { get; set; } = false;

    public void OnGet()
    {
    }

    public void OnPost()
    {
        ShowSuccess = true;
    }
}