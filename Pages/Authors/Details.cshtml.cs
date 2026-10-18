using Kellner_Lorand_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Kellner_Lorand_Lab2.Pages.Authors;

public class DetailsModel : PageModel
{
    private readonly Kellner_Lorand_Lab2Context _context;

    public DetailsModel(Kellner_Lorand_Lab2Context context)
    {
        _context = context;
    }

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
}