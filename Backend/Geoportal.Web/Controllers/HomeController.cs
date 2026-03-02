using Microsoft.AspNetCore.Mvc;

namespace Geoportal.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Graphics() => View();
    public IActionResult Report() => View();
    public IActionResult Submit() => View();
    public IActionResult Maktablar() => View();
    public IActionResult Bogchalar() => View();
    public IActionResult Tibbiyot() => View();
    public IActionResult Sport() => View();
    public IActionResult Download() => View();
    public IActionResult Login() => View();
}
