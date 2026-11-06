
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Kellner_Lorand_Lab2.Models;

namespace Kellner_Lorand_Lab2.Pages.BorrowingPages;

public class DetailsModel : PageModel
{
    private readonly Kellner_Lorand_Lab2Context _context;

    public DetailsModel(Kellner_Lorand_Lab2Context context)
    {
        _context = context;
    }

    public Borrowing Borrowing { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowing
            .Include(b => b.Member)
            .Include(b => b.Book)
                .ThenInclude(book => book!.Author)
            .FirstOrDefaultAsync(b => b.ID == id);

        if (borrowing is null)
        {
            return NotFound();
        }

        Borrowing = borrowing;

        return Page();
    }
}
