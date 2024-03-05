using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurauntApp.Models;

namespace RestaurauntApp.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
     public IActionResult Contacts()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Error404()
    {
        return View();
    }
}
