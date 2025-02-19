using Microsoft.AspNetCore.Mvc;

namespace ASP_spr321.Models.Home
{
    public class FeedbackDataForm
    {
        [FromForm(Name = "UserName")]
        public string UserName { get; set; } = null!;

        [FromForm(Name = "Tel")]
        public string Tel { get; set; } = null!;

        [FromForm(Name = "Text")]
        public string Text { get; set; } = null!;

        [FromForm(Name = "Datetime")]
        public string Datetime { get; set; } = null!;
    }
}
