using Microsoft.EntityFrameworkCore;

public class Kellner_Lorand_Lab2Context(DbContextOptions<Kellner_Lorand_Lab2Context> options) : DbContext(options)
{
    public DbSet<Kellner_Lorand_Lab2.Models.Book> Book { get; set; } = default!;
    public DbSet<Kellner_Lorand_Lab2.Models.Publisher> Publisher { get; set; } = default!;
    public DbSet<Kellner_Lorand_Lab2.Models.Author> Author { get; set; } = default!;
    public DbSet<Kellner_Lorand_Lab2.Models.Category> Category { get; set; } = default!;

}
