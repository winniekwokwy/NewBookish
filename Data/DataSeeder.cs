using NewBookish.Data;

public class DataSeeder
{
    private readonly BookishContext _context;

    public DataSeeder(BookishContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        SeedBooks();
    }

    public void SeedBooks()
    {
        if (!_context.Catalogue.Any())
        {
            var bookFaker = new BookFaker().Generate(20);
            foreach (var book in bookFaker)
            {
                _context.Catalogue.Add(book);
            }
            _context.SaveChanges();
        }
    }
}