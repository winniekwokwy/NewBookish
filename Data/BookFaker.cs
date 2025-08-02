using Bogus;
using NewBookish.Models.Entities;

public class BookFaker : Faker<Book>
{
    public BookFaker()
    {
        RuleFor(d => d.Title, f => f.Lorem.Sentence());
        RuleFor(d => d.Author, f => f.Person.FullName);
        RuleFor(d => d.NoOfCopies, f => f.Random.Number(1, 10));
        RuleFor(d => d.AvailableCopies, f => f.Random.Number(1, 1));
    }
}