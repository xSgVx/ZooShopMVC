using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Data;
using ZooShopMVC.Data;
using ZooShopMVC.Models;
using ZooShopMVC.Models.ViewModels;

namespace ZooShopMVC.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _db.Product.Include(x => x.Category)
                                                          .Include(x => x.ApplicationType);

            return View(productList);
        }

        //get - create
        public IActionResult Upsert(int? id)
        {
            /*
            IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(category => new SelectListItem
            {
                Text = category.Name,
                Value = category.Id.ToString()
            });

            ViewBag.CategoryDropDown = CategoryDropDown;
            var product = new Product();
            */

            var productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(category => new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                }),
                ApplicationTypeSelectList = _db.ApplicationType.Select(appType => new SelectListItem
                {
                    Text = appType.Name,
                    Value = appType.Id.ToString()
                })
            };

            if (id == null)
            {
                //for create
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }

                return View(productVM);
            }
        }

        //post - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                var webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    //create
                    var upload = webRootPath + WC.ImagePath;
                    var fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    _db.Product.Add(productVM.Product);
                }
                else
                {
                    //update
                    var objFromDb = _db.Product.FirstOrDefault(dbProduct => dbProduct.Id == productVM.Product.Id);

                    if (files.Count > 0)
                    {
                        //img updated
                        var upload = webRootPath + WC.ImagePath;
                        var fileName = Guid.NewGuid().ToString();
                        var extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        //img not updated
                        productVM.Product.Image = objFromDb.Image;
                    }
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            productVM.CategorySelectList = _db.Category.Select(category => new SelectListItem
            {
                Text = category.Name,
                Value = category.Id.ToString()
            });
            productVM.ApplicationTypeSelectList = _db.ApplicationType.Select(appType => new SelectListItem
            {
                Text = appType.Name,
                Value = appType.Id.ToString()
            });

            return View(productVM);
        }

        //get - delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = _db.Product.Include(x => x.Category)
                                     .Include(x => x.ApplicationType)
                                     .FirstOrDefault(x => x.Id == id);

            //product.Category = _db.Category.Find(product.CategoryId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        //post - delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var product = _db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, product.Image);

            //del image
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Product.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
