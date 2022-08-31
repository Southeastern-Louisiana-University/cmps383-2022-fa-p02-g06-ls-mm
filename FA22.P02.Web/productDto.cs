using System.ComponentModel.DataAnnotations;

namespace FA22.P02.Web
{
    public class productDto
    {
        [Required]
        int Id;
        [Required]
        [MinLength(1)]
        [MaxLength(120)]
        string? Name;
        string? Description;
        [Required]
        decimal Price;
    }
}
