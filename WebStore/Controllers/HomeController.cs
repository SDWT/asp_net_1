using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;
using Microsoft.Net.Http.Headers;



namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult BlogSingle()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult CheckOut()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Error404()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ProductDetails()
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }

        public IActionResult TestAction()
        {
            //return new ViewResult();
            //return View();

            //return new JsonResult(new { Customer = "Иванов", Id = 15, Date = DateTime.Now });
            //return Json(new { Customer = "Иванов", Id = 15, Date = DateTime.Now });

            //return Redirect("http://www.yandex.ru");
            //return new RedirectResult("http://www.yandex.ru");

            //return new RedirectToActionResult("Index", "Employees", null);
            return RedirectToAction("Index", "Employees");

            //return Content("Hello World");
            //return new ContentResult { Content = "Hello World", ContentType = "application/text" };

            //return File(Encoding.UTF8.GetBytes("Hello World!"), "application/text", "HelloWorld.txt");
            //return new FileContentResult(Encoding.UTF8.GetBytes("Hello World!"), new MediaTypeHeaderValue("application/text"));
            //return new FileStreamResult(new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")), "application/text");

            //return StatusCode(404);
            //return new StatusCodeResult(503);
            //return NoContent();
            //return new NoContentResult();
        }
    }
}