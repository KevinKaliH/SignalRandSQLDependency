using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealTime.Infrastructure.Repositorios;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductoRepositorio productoRepositorio;

        public HomeController(ILogger<HomeController> logger, IProductoRepositorio productoRepositorio)
        {
            _logger = logger;
            this.productoRepositorio = productoRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetProductos()
        {
            var data = productoRepositorio.Get().ToList();

            return Json(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
