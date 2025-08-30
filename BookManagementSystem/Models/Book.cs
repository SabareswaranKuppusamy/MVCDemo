using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; } = String.Empty;

        [Required, StringLength(100)]
        public string Author { get; set; } = String.Empty;

        [Range(0.01, 100000), DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

    }
}
