using Kellner_Lorand_Lab2.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Kellner_Lorand_Lab2.Pages.Authors;

public class IndexModel : PageModel
{
    private readonly Kellner_Lorand_Lab2Context _context;

    public IndexModel(Kellner_Lorand_Lab2Context context)
    {
        _context = context;
    }

    public IList<Author> Authors { get; set; } = new List<Author>();

    public async Task OnGetAsync()
    {
        Authors = await _context.Author
            .AsNoTracking()
            .ToListAsync();
    }
}