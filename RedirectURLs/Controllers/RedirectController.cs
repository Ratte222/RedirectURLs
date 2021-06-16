using Microsoft.AspNetCore.Mvc;
using RedirectURLs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedirectURLs.Controllers
{
    
    public class RedirectController : Controller
    {
        private UserContext db;
        public RedirectController(UserContext context)
        {
            db = context;
        }
        [Route("re/")]
        public async Task<IActionResult> Index(string sL)
        {
            Link link = await db.Links.FirstOrDefaultAsync(l => l.ShortLink == sL);
            if (link != null)
                return Redirect(link.LongLink);
            else
                return NotFound();
        }
    }
}