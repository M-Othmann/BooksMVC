using Books.Models;
using Books.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Books.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController()
        {
            _context = new ApplicationDbContext();
        }


        public IActionResult Index()
        {
            var books = _context.Books
                .Include(c => c.Category)
                .ToList();
            return View("Index", books);
        }

        public IActionResult Create()
        {
            var ViewModel = new BookFormViewModel
            {
                Categories =  _context.Categories
                .Where(m => m.IsActive)
                .ToList(),
            };
           
            return View("BookForm", ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save([Bind("Id,Title,Author,Description,CategoryId")] BookFormViewModel model)
        {
            if (!ModelState.IsValid)
            {

                model.Categories = _context.Categories.Where(m => m.IsActive).ToList();

                return View("BookForm", model);

            }
            if (model.Id == 0)
            {
                var book = new Book
                {
                    Title = model.Title,
                    Author = model.Author,
                    CategoryId = model.CategoryId,
                    Description = model.Description

                };
                _context.Books.Add(book);
            }
            else
            {
                var book = _context.Books.Find(model.Id);

                if (book is null)
                    return NotFound();

                book.Title = model.Title;
                book.Author = model.Author;
                book.CategoryId = model.CategoryId;
                book.Description = model.Description;
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Details(int? Id)
        {
            if(Id is null)
            {
                return BadRequest();
            }

            var book = _context.Books
                .Include(b => b.Category)
                .SingleOrDefault(b => b.Id == Id);

            if(book is null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var book = _context.Books.Find(id);

            if(book is null)
                return NotFound();

            var viewModel = new BookFormViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CategoryId = book.CategoryId,
                Description = book.Description,
                Categories = _context.Categories.Where(c => c.IsActive).ToList()
            };
            return View("BookForm", viewModel);

        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = _context.Books.Find(id);

            if (book is null)
                return NotFound();
            _context.Remove(book);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}
