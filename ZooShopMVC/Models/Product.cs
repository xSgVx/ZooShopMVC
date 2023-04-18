using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZooShopMVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ShortDesc { get; set; } = null!;

        [Required]
        [Range(1, double.MaxValue)]
        public double Price { get; set; }

        [ValidateNever]
        public string Image { get; set; } = null!;

        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }

        [ValidateNever]
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;

        [Display(Name = "ApplicationType Id")]
        public int ApplicationTypeId { get; set; }

        [ValidateNever]
        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; } = null!;

    }
}
