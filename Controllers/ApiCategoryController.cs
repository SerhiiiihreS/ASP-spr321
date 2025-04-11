using ASP_spr321.Data;
using ASP_spr321.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP_spr321.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class ApiCategoryController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;
        [HttpGet]
        public RestResponse Categories()
        {
            return new() 
            { 
                Service="API Products Categories",
                DataType="array",
                CacheTime=600,
                Data = _dataContext.Categories
                .AsEnumerable()
                .Select(c => c with{ImageUrl= "http://localhost:5173/Shop/Image/" + c.ImageUrl })
                .ToList(),
            };
        }
    }
}
