using ASP_spr321.Data;
using ASP_spr321.Data.Entities;
using ASP_spr321.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP_spr321.Controllers
{
    [Route("api/popularproduct")]
    [ApiController]
    public class ApiPopularProductsController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;
        [HttpGet]
       public RestResponse Products()
       {
                return new()
                {
                    Service = "API Popular Products",
                    DataType = "array",
                    CacheTime = 600,
                    Data = _dataContext.Products
                .AsEnumerable()
                .Select(c => c with { ImagesCsv = "http://localhost:5173/Shop/Image/" + c.ImagesCsv })
                .ToList(),
                };
           
       }
    }
 }

