using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kellner_Lorand_Lab2.Models;

namespace Kellner_Lorand_Lab2.Pages.BorrowingPages;

public class EditModel : PageModel
{
    private readonly Kellner_Lorand_Lab2Context _context;

    public EditModel(Kellner_Lorand_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Borrowing = await _context.Borrowing
            .FirstOrDefaultAsync(m => m.ID == id);

        if (Borrowing == null)
        {
            return NotFound();
        }

        await LoadListsAsync();

        return Page();
    }

    private async Task LoadListsAsync()
    {
        var members = await _context.Member
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .ToListAsync();

        ViewData["MemberID"] = new SelectList(
            members,
            "ID",
            "FullName",
            Borrowing.MemberID);

        var books = await _context.Book
            .Include(b => b.Author)
            .OrderBy(b => b.Title)
            .ToListAsync();

        var bookList = books.Select(b => new
        {
            b.ID,
            DisplayText = b.Title + " - " +
                (b.Author != null ? b.Author.FullName : "Unknown author")
        }).ToList();

        ViewData["BookID"] = new SelectList(
            bookList,
            "ID",
            "DisplayText",
            Borrowing.BookID);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var borrowingToUpdate = await _context.Borrowing
            .FirstOrDefaultAsync(b => b.ID == Borrowing.ID);

        if (borrowingToUpdate == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<Borrowing>(
            borrowingToUpdate,
            "Borrowing",
            b => b.MemberID,
            b => b.BookID,
            b => b.ReturnDate))
        {
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        Borrowing = borrowingToUpdate;
        await LoadListsAsync();

        return Page();
    }
}