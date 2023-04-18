namespace ZooShopMVC.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Product> Products { get; set; } = null!;
        public IEnumerable<Category> Categories { get; set; } = null!;


    }
}
