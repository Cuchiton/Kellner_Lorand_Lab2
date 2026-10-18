using Kellner_Lorand_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Kellner_Lorand_Lab2.Pages.Authors;

public class EditModel : PageModel
{
    private readonly Kellner_Lorand_Lab2Context _context;

    public EditModel(Kellner_Lorand_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Author Author { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var author = await _context.Author.FindAsync(id);

        if (author == null)
            return NotFound();

        Author = author;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var author = await _context.Author.FindAsync(Author.ID);

        if (author == null)
            return NotFound();

        author.FirstName = Author.FirstName;
        author.LastName = Author.LastName;

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}