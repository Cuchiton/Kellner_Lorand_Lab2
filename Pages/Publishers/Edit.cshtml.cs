using Kellner_Lorand_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Kellner_Lorand_Lab2.Pages.Publishers;

public class EditModel : PageModel
{
    private readonly Kellner_Lorand_Lab2Context _context;

    public EditModel(Kellner_Lorand_Lab2Context context)
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

        var publisher = await _context.Publisher.FindAsync(id);

        if (publisher == null)
        {
            return NotFound();
        }

        Publisher = publisher;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Publisher).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Publisher.AnyAsync(p => p.ID == Publisher.ID))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }
}