﻿using Microsoft.AspNetCore.Mvc;

namespace ASP_spr321.Models.Home
{
    public class HomeModelsFormModel
    {
        [FromForm(Name = "user-name")]
        public string UserName { get; set; } = null!;


        [FromForm(Name = "user-email")]
        public string UserEmail { get; set; } = null!;

    }
}
