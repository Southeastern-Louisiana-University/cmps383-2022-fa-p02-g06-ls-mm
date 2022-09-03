using System.ComponentModel.DataAnnotations;
namespace FA22.P02.Web.Features.Products;

    public class ProductDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(120)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
    }

