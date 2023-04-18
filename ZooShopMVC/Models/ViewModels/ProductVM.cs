using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ZooShopMVC.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; } = null!;

        [ValidateNever]
        public IEnumerable<SelectListItem> CategorySelectList { get; set; } = null!;

        [ValidateNever]
        public IEnumerable<SelectListItem> ApplicationTypeSelectList { get; set; } = null!;
    }
}
