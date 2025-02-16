using Microsoft.AspNetCore.Mvc;

namespace ASP_spr321.Models.Home
{
    public class HomeModelsAjaxModel
    {
        [ModelBinder(Name="userName")]
        public string UserName { get; set; } = null!;

        [ModelBinder(Name = "userEmail")]
        public string UserEmail { get; set; } = null!;
    }
}
