using BCASLibrary.Models;
using BCASLibrary.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BCASLibrary.Controllers
{
    [Authorize]

    public class BookController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Book> bookList = _db.Book.Include(u => u.Genre).ToList();
            return View(bookList);
        }
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            BookVM bookVM = new()
            {
                GenreList = _db.Genre.ToList().Select(u => new SelectListItem
                {
                    Text = u.GenreName,
                    Value = u.Id.ToString()
                }),
                Book = new Book()
            };
            if (id == null || id == 0)
            {
                return View(bookVM);
            }
            else
            {
                bookVM.Book = _db.Book.First(u => u.Id == id);
                return View(bookVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(BookVM bookVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); 
                    string bookPath = Path.Combine(wwwRootPath, @"images\book");

                    if (!string.IsNullOrEmpty(bookVM.Book.ImageUrl))
                    {
                        //delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, bookVM.Book.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    
                    }

                    using ( var fileStream = new FileStream(Path.Combine(bookPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    bookVM.Book.ImageUrl = @"\images\book\" + fileName;
                }

                if(bookVM.Book.Id == 0)
                {
                    _db.Book.Add(bookVM.Book);
                    TempData["success"] = "Book Created Successfully!";

                    _db.SaveChanges();
                }
                else
                {
                    _db.Book.Update(bookVM.Book);
                    TempData["success"] = "Book Updated Successfully!";

                    _db.SaveChanges();

                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                bookVM.GenreList = _db.Genre.ToList().Select(u => new SelectListItem
                {
                    Text = u.GenreName,
                    Value = u.Id.ToString()
                });
            }
            return View(bookVM);

        }

        #region API CALLS
        //book
        [HttpGet]
        public IActionResult ToList()
        {
            List<Book> bookList = _db.Book.Include(u => u.Genre).ToList();
            return Json(new { data = bookList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var bookToBeDelete = _db.Book.Include(u => u.Genre).FirstOrDefault(u=>u.Id==id);
            if(bookToBeDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, bookToBeDelete.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _db.Book.Remove(bookToBeDelete);
            _db.SaveChanges();

            return Json(new { success = true, message = "Deleted successfully" });
        }

        #endregion

    }
}
