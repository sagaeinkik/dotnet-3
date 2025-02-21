using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookApp.Data;
using BookApp.Models;

namespace BookApp.Controllers
{
    public class AuthorController : Controller
    {
        //Databasanslutning
        private readonly BookAppContext _context;

        public AuthorController(BookAppContext context)
        {
            _context = context;
        }

        //Hämta alla författare
        [Route("authors")]
        public async Task<IActionResult> Index()
        {
            //Returnera alla i lista
            return View(await _context.Authors.ToListAsync());
        }

        // Hämta enskild författare
        [Route("authors/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Hitta författare
            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            //Skicka till vy
            return View(author);
        }

        // Skapa-formulär
        [Route("authors/new")]
        public IActionResult Create()
        {
            return View();
        }

        // Skapa - POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("authors/new")]
        public async Task<IActionResult> Create([Bind("Id,FullName,Bio")] Author author)
        {
            //Validering
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // Redigera-formulär
        [Route("authors/edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // Redigera - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("authors/edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Bio")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            //Validera
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //Omdirigera tillbaka till lista över alla
                return RedirectToAction(nameof(Index));
            }
            //Visa fel
            return View(author);
        }

        // Delete-vy med detaljer
        [Route("authors/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Hitta författare
            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // Delete författare - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("authors/delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Hitta
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                //Radera
                _context.Authors.Remove(author);
            }

            //Spara
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
