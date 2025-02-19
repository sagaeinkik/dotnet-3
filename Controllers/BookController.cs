using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookApp.Data;
using BookApp.Models;

namespace BookApp.Controllers
{
    public class BookController : Controller
    {
        private readonly BookAppContext _context;

        public BookController(BookAppContext context)
        {
            _context = context;
        }

        //Alla böcker
        [Route("/books")]
        public async Task<IActionResult> Index()
        {
            //Hämta alla böcker, med författarna (författare är navigeringsegenskap)
            var bookAppContext = _context.Books.Include(b => b.Author);
            //skicka som lista till vy
            return View(await bookAppContext.ToListAsync());
        }

        //Enskild bok
        [Route("/books/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            //Kolla om ID är null (finns ej)
            if (id == null)
            {
                return NotFound();
            }

            //Hämta bok med författare
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        //Create-formulär
        [Route("/books/create")]
        public IActionResult Create()
        {
            //Skicka med författarnamnen till select-listan istället för bara ID
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName");
            return View();
        }

        //Skapa bok - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Type,Genre,Blurb,AuthorId")] Book book)
        {
            //Validera
            if (ModelState.IsValid)
            {
                //Lägg till, spara och omdirigera till lista över alla böcker
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //Validering ogiltig, skapa om select-listan med författarnamn och skicka tillbaka till formuläret
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", book.AuthorId);
            return View(book);
        }

        //Bok att redigera
        [Route("/books/edit/{id}")]
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

        //Redigera bok - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Type,Genre,Blurb,AuthorId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            //Validering
            if (ModelState.IsValid)
            {
                //Osäker på varför scaffoldingen genererar en try-catch här men inte på create?
                try
                {
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

                //Allt gick bra, skicka till listan över alla böcker
                return RedirectToAction(nameof(Index));
            }

            //Validering misslyckad
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", book.AuthorId);
            return View(book);
        }

        // Bok att radera
        [Route("/books/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Hitta boken
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // Bok att radera - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                //Radera boken
                _context.Books.Remove(book);
            }

            //Spara ändringar, returnera till listan över alla böcker
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
