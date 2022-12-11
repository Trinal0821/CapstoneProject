using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
/*using OutlookExecutable;*/
using System.Diagnostics;
using EAServer.Models;
using Newtonsoft;
using Newtonsoft.Json;
using EAServer.Models;

namespace EAServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public IActionResult TaskPane()
        {

            return View();
        }

        public IActionResult FunctionFile()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Run()
        {

            List<string> names = new List<string>();
            names.Add("Trina");
            names.Add("Allyanna");

            string json = JsonConvert.SerializeObject(names);
            return Content(json);

            /*  if (1 == 1)
              {
                  Console.WriteLine("hELLO");
              }*/

            //return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}