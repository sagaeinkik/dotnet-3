using System.ComponentModel.DataAnnotations;

namespace BookApp.Models;

public class Author
{
    public int Id { get; set; }

    [Required]
    public string? FullName { get; set; }

    public string? Bio { get; set; }

    //Lista med böcker som författaren har skrivit
    public List<Book>? Books { get; set; }
}
