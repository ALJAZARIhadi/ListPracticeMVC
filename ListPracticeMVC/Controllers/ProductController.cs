using System.Diagnostics;
using ListPracticeMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ListPracticeMVC.Controllers
{
    
    public class ProductController : Controller
    {
        // 1.First Step: Create Action Method to show the list of products (GET Request)
        public IActionResult ProductsList()
        {
            // 2.Second Step: Get the list of products from the FakeDatabase and pass it to the view (using ViewBag, ViewData, or as a model)
            var allProducts = FakeDatabase.Products.ToList();
            return View(allProducts);
        }

        [HttpGet] // 
        public IActionResult ProductAdd()
        {
            return View();
        }
        // Notice that we have two action methods with the same name but different HTTP verbs (GET and POST) - this is called method overloading and it's a common practice in MVC for handling form submissions.
        [HttpPost]
        public IActionResult ProductAdd(Product newProduct)
        {
            // 1. define a new ID for the product (since we are not using a real database,
            // we need to manually assign an ID to the new product. In a real database,
            // this would be handled automatically with an auto-incrementing primary key).
            // We can simply take the maximum existing ID and add 1 to it, or start from 1 if there are no products yet.
            int newId = 1;
            if (FakeDatabase.Products.Any())
            {
                newId = FakeDatabase.Products.Max(p => p.Id) + 1;
            }

            newProduct.Id = newId;

            // 2. Add the new product to the FakeDatabase (which is just a static list in memory).
            FakeDatabase.Products.Add(newProduct);

            // 3. Redirect the user back to the ProductsList action to see the updated list of products.
            // We use RedirectToAction to perform a redirect to another action method,
            // which will trigger a new GET request to that action and display the updated list of products.
            return RedirectToAction("ProductsList");
        }


        // ==========================================
        //  DELETE (Details & Delete - ProductDelete)
        // ==========================================

        [HttpGet]
        public IActionResult ProductDelete(int id)
        {
            // Get the product details before deleting
            var product = FakeDatabase.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // We use ActionName because C# doesn't allow two methods with the exact same signature (int id)
        [HttpPost, ActionName("ProductDelete")]
        public IActionResult ProductDeleteConfirmed(int id)
        {
            // Find the product and remove it from the list
            var product = FakeDatabase.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                FakeDatabase.Products.Remove(product);
            }
            return RedirectToAction("ProductsList");
        }

        // ==========================================
        //  UPDATE (Edit - ProductUpdateDelete)
        // ==========================================
        [HttpGet]
        public IActionResult ProductUpdateDelete(int id)
        {
            // Find the product to fill the form
            var product = FakeDatabase.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        public IActionResult ProductUpdateDelete(Product updatedProduct)
        {
            // Find the old product and update its properties
            var existingProduct = FakeDatabase.Products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = updatedProduct.Name;
                existingProduct.Price = updatedProduct.Price;
                existingProduct.Description = updatedProduct.Description;
                existingProduct.ImageUrl = updatedProduct.ImageUrl;
            }
            return RedirectToAction("ProductsList");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}