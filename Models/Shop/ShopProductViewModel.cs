using ASP_spr321.Data.Entities;

namespace ASP_spr321.Models.Shop
{
    public class ShopProductViewModel
    {
        public Product? Product {  get; set; }
        public List<Product> Products { get; set; } = [];
        public List<BreadCrumb> BreadCrumbs { get; set; } = [];
    }
}
