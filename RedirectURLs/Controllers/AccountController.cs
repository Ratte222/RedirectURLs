using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RedirectURLs.ViewModels; // пространство имен моделей RegisterModel и LoginModel
using RedirectURLs.Models; // пространство имен UserContext и класса User
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace RedirectURLs.Controllers
{
    public class AccountController : Controller
    {
        private UserContext db;
        public AccountController(UserContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email &&
                    u.Password == Crypt.GetHashSHA512(model.Password));
                if (user != null)
                {
                    await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    db.Users.Add(new User { Email = model.Email, 
                        Password = Crypt.GetHashSHA512(model.Password) });
                    await db.SaveChangesAsync();
                    await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> ViewLinks()
        {
            return View(await db.Links.ToListAsync());
            
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
            if(user == null) return NotFound();
            link.ShortLink = Convert.ToString(db.Links.Count(i=> i.ClientId == user.Id)+1, 16);
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