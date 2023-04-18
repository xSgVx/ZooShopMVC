using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZooShopMVC.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required] 
        [DisplayName("Display order")]
        [Range(1, int.MaxValue, ErrorMessage ="Display Order for category must be greater than 0")]
        public int DisplayOrder { get; set; }




    }
}
