using ASP_spr321.Models.Admin;

namespace ASP_spr321.Data.Entities
{
    public record Category
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public String Name { get; set; } = null!;
        public String Description { get; set; } = null!;
        public String Slug { get; set; } = null!;
        public String ImageUrl { get; set; } = null!;

        public Category ParentCategory { get; set; }=null!;
        public List<Product> Products { get; set; } = [];
        public Product Product { get; set; } = null!;

    }
}
