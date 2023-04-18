using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ZooShopMVC.Data;
using ZooShopMVC.Models;

namespace ZooShopMVC.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            IEnumerable<ApplicationType> appTypesList = _db.ApplicationType;
            return View(appTypesList);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType appType)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Add(appType);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appType);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var appType = _db.ApplicationType.Find(id);

            if (appType == null)
            {
                return NotFound();
            }

            return View(appType);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(ApplicationType appType)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(appType);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appType);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var appType = _db.ApplicationType.Find(id);
            if (appType == null)
            {
                return NotFound();
            }

            return View(appType);
        }

        //post - delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var appType = _db.ApplicationType.Find(id);
            if (appType == null)
            {
                return NotFound();
            }

            _db.ApplicationType.Remove(appType);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
