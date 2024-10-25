using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodRegistrationTool.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Nutrition { get; set; } = string.Empty;
        // Maybe not neccessary
        public decimal Price { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }
        public string? NutriScore { get; set; }

        // Doesn't put class in DB
        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }

        // Navigation property
        //public virtual List<ProductRegister>? ProductRegisters { get; set; }

        // Hvis det legges til nye klasser, kjør følgende i terminal (se async modulen i canvas):
        // dotnet ef migrations add FoodRegistryDbExpanded
        // dotnet ef database update 
    }
}