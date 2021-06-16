using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedirectURLs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedirectURLs.Controllers
{
    [Authorize]
    public class ManageLinkController : Controller
    {
        private UserContext db;
        public ManageLinkController(UserContext context)
        {
            db = context;
        }
        public async Task<IActionResult> ViewLinks()
        {
            User user = await db.Users.FirstOrDefaultAsync(
                u => u.Email == HttpContext.User.Identity.Name);
            if (user == null)
                return NotFound();
            else
                return View(db.Links.Where(l => l.ClientId == user.Id).Select(l=>l)/*.ToListAsync()*/);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Link link)
        {
            User user = await db.Users.FirstOrDefaultAsync(
                u => u.Email == HttpContext.User.Identity.Name);
            if (user == null) return NotFound();
            //link.ShortLink = $"https://localhost:44330/re?sL={Convert.ToString(db.Links.Count(i => i.ClientId == user.Id) + 1, 16)}";
            if(db.Links.Count()>0)
                link.ShortLink = Convert.ToString(db.Links.OrderBy(i=>i.Id).Last().Id + 1, 16);
            else
                link.ShortLink = Convert.ToString(0, 16);
            link.ClientId = user.Id;
            db.Links.Add(link);
            await db.SaveChangesAsync();
            return RedirectToAction("ViewLinks");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                Link link = await db.Links.FirstOrDefaultAsync(p => p.Id == id);
                if (link != null)
                    return View(link);
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Link link = await db.Links.FirstOrDefaultAsync(p => p.Id == id);
                if (link != null)
                    return View(link);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Link link)
        {
            Link _link = await db.Links.FirstOrDefaultAsync(p => p.Id == link.Id);
            _link.LongLink = link.LongLink;//отсутствовала часть данных в пришедшем объекте,
            //добавил такой костыть чтоб всё правильно работало
            db.Links.Update(_link);
            await db.SaveChangesAsync();
            return RedirectToAction("ViewLinks");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Link link = await db.Links.FirstOrDefaultAsync(p => p.Id == id);
                if (link != null)
                    return View(link);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Link link = await db.Links.FirstOrDefaultAsync(p => p.Id == id);
                if (link != null)
                {
                    db.Links.Remove(link);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ViewLinks");
                }
            }
            return NotFound();
        }
    }
}
