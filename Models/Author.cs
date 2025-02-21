using System.ComponentModel.DataAnnotations;

namespace BookApp.Models;

public class Author
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Du måste fylla i författarens namn")]
    [Display(Name = "Namn")]
    public string? FullName { get; set; }

    [Display(Name = "Biografi")]
    public string? Bio { get; set; }

    //Lista med böcker som författaren har skrivit
    public List<Book>? Books { get; set; }
}
