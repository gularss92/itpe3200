using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodRegistrationTool.ViewModels;

public class ProductCreateViewModel
{
    [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The name must be numbers or letters and between 2 to 20 chars.")]
    [Display(Name = "Product name")]
    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;
    public string Nutrition { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "The price must be > 0.")]
    public decimal Price { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    [Display(Name = "Image")]
    public string? ImageUrl { get; set; }

    public string? NutriScore { get; set; }

    [NotMapped]
    [Display(Name = "Upload Image")]
    public IFormFile? ImageFile { get; set; }

    public int ProducerId { get; set; }
}