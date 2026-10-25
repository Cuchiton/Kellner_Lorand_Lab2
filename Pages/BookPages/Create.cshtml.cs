using Kellner_Lorand_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kellner_Lorand_Lab2.Pages.BookPages;

public class CreateModel : BookCategoriesPageModel
{
    private readonly Kellner_Lorand_Lab2Context _context;

    public CreateModel(Kellner_Lorand_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = new();

    public SelectList? AuthorList { get; set; }

    private async Task LoadAuthorsAsync()
    {
        var authors = await _context.Author
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .Select(a => new
            {
                a.ID,
                FullName = a.LastName + " " + a.FirstName
            })
            .ToListAsync();

        AuthorList = new SelectList(authors, "ID", "FullName");
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Book = new Book();
        Book.BookCategories = new List<BookCategory>();

        PopulateAssignedCategoryData(_context, Book);

        await LoadAuthorsAsync();

        ViewData["PublisherID"] = new SelectList(
            _context.Publisher,
            "ID",
            "PublisherName");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
    {
        if (selectedCategories != null)
        {
            Book.BookCategories = new List<BookCategory>();

            foreach (var category in selectedCategories)
            {
                Book.BookCategories.Add(new BookCategory
                {
                    CategoryID = int.Parse(category)
                });
            }
        }

        if (!ModelState.IsValid)
        {
            PopulateAssignedCategoryData(_context, Book);
            await LoadAuthorsAsync();

            ViewData["PublisherID"] = new SelectList(
                _context.Publisher,
                "ID",
                "PublisherName",
                Book.PublisherID);

            return Page();
        }

        _context.Book.Add(Book);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}