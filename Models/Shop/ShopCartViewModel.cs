using ASP_spr321.Data.Entities;

namespace ASP_spr321.Models.Shop
{
    public class ShopCartViewModel
    {
        public Cart? Cart {  get; set; }

        public CartItem? CartItem { get; set; }

        public List<Product> Products { get; set; } = [];
        public List<BreadCrumb> BreadCrumbs { get; set; } = [];
    }
}
