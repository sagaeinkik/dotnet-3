using System.ComponentModel.DataAnnotations;

namespace BookApp.Models;

public class Book
{
    public int Id { get; set; }

    //Boktitel
    [Required(ErrorMessage = "Fyll i bokens titel")]
    [Display(Name = "Titel")]
    public string? Title { get; set; }

    //Skön- eller facklitteratur
    [Required(ErrorMessage = "Fyll i bokens typ")]
    [Display(Name = "Typ")]
    public BookType? Type { get; set; }

    //Genre (t.ex. fantasy, deckare, historia)
    public string? Genre { get; set; }

    //Baksidan på boken
    [Display(Name = "Beskrivning")]
    public string? Blurb { get; set; }

    //Författare
    [Required(ErrorMessage = "Ange författare")]
    public int? AuthorId { get; set; }

    public Author? Author { get; set; }
}