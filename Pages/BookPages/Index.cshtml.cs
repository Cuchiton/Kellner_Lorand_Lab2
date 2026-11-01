using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Kellner_Lorand_Lab2.Models;

namespace Kellner_Lorand_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Kellner_Lorand_Lab2Context _context;

    public IndexModel(Kellner_Lorand_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = new List<Book>();

    public BookData BookD { get; set; } = new BookData();

    public int BookID { get; set; }

    public int CategoryID { get; set; }

    public string TitleSort { get; set; } = "";

    public string AuthorSort { get; set; } = "";

    public string? CurrentFilter { get; set; }

    public async Task OnGetAsync(
        int? id,
        int? categoryID,
        string? sortOrder,
        string? searchString)
    {
        BookD = new BookData();

        TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
        AuthorSort = sortOrder == "author" ? "author_desc" : "author";

        CurrentFilter = searchString;

        BookD.Books = await _context.Book
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync();

        // Cautare dupa titlul cartii sau numele complet al autorului
        if (!String.IsNullOrWhiteSpace(searchString))
        {
            string searchText = searchString.Trim();

            BookD.Books = BookD.Books.Where(s =>
                s.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                ||
                (s.Author != null &&
                 s.Author.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            );
        }

        if (id != null)
        {
            BookID = id.Value;

            Book book = BookD.Books
                .Where(i => i.ID == id.Value)
                .Single();

            BookD.Categories = book.BookCategories
                .Select(s => s.Category);
        }

        switch (sortOrder)
        {
            case "title_desc":
                BookD.Books = BookD.Books
                    .OrderByDescending(s => s.Title);
                break;

            case "author_desc":
                BookD.Books = BookD.Books
                    .OrderByDescending(s => s.Author.FullName);
                break;

            case "author":
                BookD.Books = BookD.Books
                    .OrderBy(s => s.Author.FullName);
                break;

            default:
                BookD.Books = BookD.Books
                    .OrderBy(s => s.Title);
                break;
        }

        if (categoryID != null)
        {
            CategoryID = categoryID.Value;

            Book = BookD.Books
                .Where(b => b.BookCategories
                    .Any(bc => bc.CategoryID == categoryID.Value))
                .ToList();
        }
        else
        {
            Book = BookD.Books.ToList();
        }
    }
}