using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookApp.Data;
using BookApp.Models;

namespace BookApp.Controllers
{
    public class BookController : Controller
    {
        //Databasanslutning
        private readonly BookAppContext _context;

        public BookController(BookAppContext context)
        {
            _context = context;
        }

        // Alla böcker
        [Route("books")]
        public async Task<IActionResult> Index()
        {
            //Inkludera författare (navigeringsegenskap)
            var bookAppContext = _context.Books.Include(b => b.Author);
            return View(await bookAppContext.ToListAsync());
        }

        // Enskild bok
        [Route("books/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Hitta bok med författare
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // Skapa-formulär
        [Route("books/new")]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName");
            return View();
        }

        // Skapa - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("books/new")]
        public async Task<IActionResult> Create([Bind("Id,Title,Type,Genre,Blurb,AuthorId")] Book book)
        {
            //Validera, spara och skicka tillbaka till Index
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", book.AuthorId);
            return View(book);
        }

        // Redigera-formulär
        [Route("books/edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", book.AuthorId);
            return View(book);
        }

        // Redigera - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("books/edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Type,Genre,Blurb,AuthorId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            //Validera
            if (ModelState.IsValid)
            {
                try
                {
                    //Uppdatera och spara
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //Omdirigera till alla böcker
                return RedirectToAction(nameof(Index));
            }
            //Skicka med författare till vyn
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", book.AuthorId);
            return View(book);
        }

        // Radera-vy
        [Route("books/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // Radera - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("books/delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
