﻿using F88.Digital.Web.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace F88.Digital.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class HomeController : BaseController<HomeController>
    {
        public IActionResult Index()
        {
            _notify.Information("Hi There!");
            return View();
        }
    }
}