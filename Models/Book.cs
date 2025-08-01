public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }

    public required int NoOfCopies { get; set; }

    public required int AvailableCopies { get; set; }

    public List<Member> Borrowers { get; set; } = new List<Member>();

}