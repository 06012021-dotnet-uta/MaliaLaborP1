using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project1DbContext;

namespace Project1.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer objUser) // need to do something with sessions here?
        {
            if (ModelState.IsValid)
            {
                using (Project1DBContext db = new Project1DBContext())
                {
                    var obj = db.Customers.Where(x => x.Username.Equals(objUser.Username) && x.Password.Equals(objUser.Password)).FirstOrDefault();
                    return View(obj);
                }
            }
            return NotFound();
        }
    }
}
