using System.ComponentModel.DataAnnotations;

public class Review
{
    public int Id { get; set; }

    [Required]
    public int EventId { get; set; }

    [Required]
    [StringLength(1000)]
    public required string Content { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    public required Event Event { get; set; }
}
