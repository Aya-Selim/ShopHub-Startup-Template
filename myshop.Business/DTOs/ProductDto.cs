using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myshop.Business.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [DisplayName("Image")]
        public string? Img { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
    }
}
