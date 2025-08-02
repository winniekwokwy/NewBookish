using NewBookish.Data;
using Bogus;
using NewBookish.Models.Entities;

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
        if (!_context.Books.Any())
        {
            var faker = new Faker<Book>("en")
                        .RuleFor(d => d.Title, f => f.Lorem.Sentence())
                        .RuleFor(d => d.Author, f => f.Person.FullName)
                        .RuleFor(d => d.NoOfCopies, f => f.Random.Number(1, 10))
                        .RuleFor(d => d.AvailableCopies, f => f.Random.Number(1, 1))
                        .Generate(20);
            foreach (var book in faker)
            {
                _context.Books.Add(book);
            }
            _context.SaveChanges();
        }
    }
}