using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Kellner_Lorand_Lab2.Models;

namespace Kellner_Lorand_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Kellner_Lorand_Lab2Context _context;

    public IndexModel(Kellner_Lorand_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; } = new List<Category>();

    public IList<Book> Books { get; set; } = new List<Book>();

    public int? CategoryID { get; set; }

    public async Task OnGetAsync(int? id)
    {
        Category = await _context.Category
            .AsNoTracking()
            .OrderBy(c => c.CategoryName)
            .ToListAsync();

        if (id != null)
        {
            CategoryID = id.Value;

            Books = await _context.Book
                .Include(b => b.Author)
                .Where(b => b.BookCategories
                    .Any(bc => bc.CategoryID == id.Value))
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .ToListAsync();
        }
    }
}