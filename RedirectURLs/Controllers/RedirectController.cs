using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedirectURLs.Controllers
{
    public class RedirectController : Controller
    {
        public IActionResult Index(string shortURL)
        {

            return Redirect("https://www.youtube.com/c/%D0%9F%D1%80%D0%BE%D0%B3%D1%80%D0%B0%D0%BC%D1%8B%D1%81%D0%BB%D0%B8%D0%92%D0%B8%D0%B4%D0%B5%D0%BE%D1%83%D1%80%D0%BE%D0%BA%D0%B8/videos");
        }
    }
}