using Microsoft.AspNetCore.Mvc;

namespace ASP_spr321.Models.Home
{
    public class FeedbackDataJson
    {
        [ModelBinder(Name = "UserName")]
        public string UserName { get; set; } = null!;

        [ModelBinder(Name = "Tel")]
        public string Tel { get; set; } = null!;

        [ModelBinder(Name = "Text")]
        public string Text { get; set; } = null!;

        [ModelBinder(Name = "Datetime")]
        public string Datetime { get; set; } = null!;
    }
}
