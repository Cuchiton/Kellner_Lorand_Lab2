using Kellner_Lorand_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Kellner_Lorand_Lab2.Pages.Authors;

public class DeleteModel : PageModel
{
    private readonly Kellner_Lorand_Lab2Context _context;

    public DeleteModel(Kellner_Lorand_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Author Author { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var author = await _context.Author
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.ID == id);

        if (author == null)
            return NotFound();

        Author = author;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var author = await _context.Author.FindAsync(Author.ID);

        if (author == null)
            return RedirectToPage("./Index");

        
        var hasBooks = await _context.Book
            .AnyAsync(b => b.AuthorID == Author.ID);

        if (hasBooks)
        {
            ModelState.AddModelError(string.Empty,
                "This author has books assigned. Reassign or delete those books first.");

            Author = author;
            return Page();
        }

        _context.Author.Remove(author);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}