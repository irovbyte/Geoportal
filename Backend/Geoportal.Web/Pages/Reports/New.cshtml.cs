using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Geoportal.Web.Pages.Reports;

public class NewModel : PageModel
{
    [BindProperty] public string? Category { get; set; }
    [BindProperty] public string? Address { get; set; }
    [BindProperty] public string? Description { get; set; }
    [BindProperty] public string? Contact { get; set; }

    public bool ShowSuccess { get; private set; }

    public void OnGet() => ShowSuccess = false;

    public IActionResult OnPost()
    {
        ShowSuccess = true;
        return Page();
    }
}
