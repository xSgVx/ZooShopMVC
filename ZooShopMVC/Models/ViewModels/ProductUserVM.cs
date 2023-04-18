using Microsoft.AspNetCore.Mvc;

namespace ZooShopMVC.Models.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductList = new List<Product>();
        }

        public ApplicationUser ApplicationUser { get; set; } = null!;
        public IList<Product> ProductList { get; set; }
    }
}
