using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothesStore.Models;
using ClothesStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClothesStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            this._productService = productService;
        }

        public IActionResult Index()
        {
            return View(_productService.Get());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
             _productService.Create(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(string id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _productService.Update(id, product);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                var product = _productService.Get(id);

                if (product == null)
                {
                    return NotFound();
                }

                _productService.Remove(product.Id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public ActionResult AttachImage(string id)
        {
            var rental = _productService.Get(id);
            return View(rental);
        }
    }
}