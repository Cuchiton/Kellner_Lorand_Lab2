using Kellner_Lorand_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Kellner_Lorand_Lab2.Pages.Publishers;

public class DeleteModel : PageModel
{
    private readonly Kellner_Lorand_Lab2Context _context;

    public DeleteModel(Kellner_Lorand_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Publisher Publisher { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var publisher = await _context.Publisher
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ID == id);

        if (publisher == null)
        {
            return NotFound();
        }

        Publisher = publisher;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var publisher = await _context.Publisher.FindAsync(Publisher.ID);

        if (publisher != null)
        {
            _context.Publisher.Remove(publisher);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}