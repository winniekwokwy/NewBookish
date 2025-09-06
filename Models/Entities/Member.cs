namespace NewBookish.Models.Entities;

public class Member : User
{
    public int MemberId { get; set; }
    public string? Email { get; set; }
    public required string PhoneNumber { get; set; }

    public List<Book> LoanedBooks { get; set; } = [];
}