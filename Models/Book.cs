using System.ComponentModel.DataAnnotations;

namespace BookApp.Models;

public class Book
{
    public int Id { get; set; }

    //Boktitel
    [Required]
    public string? Title { get; set; }

    //Skön- eller facklitteratur
    [Required]
    public string? Type { get; set; }

    //Genre (t.ex. fantasy, deckare, historia)
    public string? Genre { get; set; }

    //Baksidan på boken
    public string? Blurb { get; set; }

    //Författare
    [Required]
    public int? AuthorId { get; set; }

    public Author? Author { get; set; }
}