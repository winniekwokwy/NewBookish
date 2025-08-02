using NewBookish.Models.Entities;

namespace NewBookish.Models;

public class CatalogueViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public List<Book> Books { get; set; } = new List<Book>();
}
