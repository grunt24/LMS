using BCASLibrary.Models;
using BCASLibrary.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BCASLibrary.Controllers
{
    [Authorize]
    public class GenreController : Controller
    {
        private readonly AppDbContext _db;

        public GenreController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Genre> genreList = _db.Genre.ToList();
            return View(genreList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            GenreVM genreVM = new()
            {
                Genre = new Genre()
            };
            if (id == null || id == 0)
            {
                return View(genreVM);
            }
            else
            {
                genreVM.Genre = _db.Genre.First(u => u.Id == id);
                return View(genreVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(GenreVM genreVM)
        {
            if (ModelState.IsValid)
            {
                if (genreVM.Genre.Id == 0)
                {
                    _db.Genre.Add(genreVM.Genre);
                    TempData["success"] = "Book Created Successfully!";

                    _db.SaveChanges();
                }
                else
                {
                    _db.Genre.Update(genreVM.Genre);
                    TempData["success"] = "Book Updated Successfully!";

                    _db.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(genreVM);

        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Create(Genre model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Genre.Add(model);
        //        _db.SaveChanges();
        //        TempData["success"] = "Genre Created Successfully!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}
        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    if(id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Genre? genreFromDb = _db.Genre.Find(id);

        //    if(genreFromDb == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(genreFromDb);
        //}
        //[HttpPost]
        //public IActionResult Edit(Genre model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Genre.Update(model);
        //        _db.SaveChanges();
        //        TempData["success"] = "Genre Updated Successfully!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}

        #region API CALLS
        //book
        [HttpGet]
        public IActionResult ToList()
        {
            List<Genre> genreList = _db.Genre.ToList();
            return Json(new { data = genreList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var genreToBeDelete = _db.Genre.Find(id);
            if (genreToBeDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _db.Genre.Remove(genreToBeDelete);
            _db.SaveChanges();
            return Json(new { success = true, message = "Deleted successfully" });
        }

        #endregion


    }
}
