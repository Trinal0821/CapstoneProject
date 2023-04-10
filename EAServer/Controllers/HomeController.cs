using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
using EAServer.Models;
using Newtonsoft;
using Newtonsoft.Json;
using OutlookExecutable;

namespace EAServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public IActionResult DirectorySelector()
        {
            return View();
        }

        public IActionResult Info()
        {

            return View();
        }

        public IActionResult RetagEmail()
        {

            return View();
        }

        public IActionResult Settings()
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

        public IActionResult Temp()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult getTag(string from, string subject, string body)
        {
            Classifier classifier = new Classifier();
            return Content(classifier.execute(from, subject, body));
        }

        [HttpPost("/Home/sendEmails")]
        public IActionResult sendEmails()
        {

            var files = Request.Form.Files;
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files were uploaded.");
            }
            
            FolderSystem folder = new FolderSystem();
            foreach(var file in files)
            {
                
            }
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}