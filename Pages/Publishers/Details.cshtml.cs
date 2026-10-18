using Kellner_Lorand_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Kellner_Lorand_Lab2.Pages.Publishers;

public class DetailsModel : PageModel
{
    private readonly Kellner_Lorand_Lab2Context _context;

    public DetailsModel(Kellner_Lorand_Lab2Context context)
    {
        _context = context;
    }

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
}