using Kellner_Lorand_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Kellner_Lorand_Lab2.Pages.BookPages;

public class CreateModel : PageModel
{
    public Microsoft.AspNetCore.Mvc.Rendering.SelectList? AuthorList { get; set; }
    private readonly Kellner_Lorand_Lab2Context _context;

    public CreateModel(Kellner_Lorand_Lab2Context context)
    {
        _context = context;
    }
    private async Task LoadAuthorsAsync()
    {
        var authors = await _context.Author
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .Select(a => new
            {
                a.ID,
                FullName = a.FirstName + " " + a.LastName
            })
            .ToListAsync();

        AuthorList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            authors, "ID", "FullName");
    }



    public async Task<IActionResult> OnGetAsync()
    {
        await LoadAuthorsAsync();

        ViewData["PublisherID"] = new SelectList(
            _context.Set<Publisher>(),
            "ID",
            "PublisherName");

        return Page();
    }



    [BindProperty]
    public Book Book { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {

        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Book.Add(Book);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
