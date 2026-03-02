using Microsoft.AspNetCore.Mvc;

namespace Geoportal.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Graphics()
    {
        return View();
    }
    public IActionResult Report()
    {
        return View();
    }
    public IActionResult Submit()
    {
        return View();
    }
    public IActionResult Maktablar()
    {
        return View();
    }
    public IActionResult Bogchalar() => View();
    public IActionResult Tibbiyot() => View();
    public IActionResult Sport() => View();
}
