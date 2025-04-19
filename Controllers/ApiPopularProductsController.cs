using ASP_spr321.Data;
using ASP_spr321.Data.Entities;
using ASP_spr321.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP_spr321.Controllers
{
    [Route("api/popularproduct")]
    [ApiController]
    public class ApiPopularProductsController(DataAccessor dataAccessor) : ControllerBase
    {
        private readonly DataAccessor _dataAcccessor = dataAccessor;

        [HttpGet]
        public RestResponse PopProducts()
        {
            return new()
            {
                Service = "API Products",
                DataType = "array",
                CacheTime = 600,
                Data = _dataAcccessor.PopularProducts(),
            };
        }
    } 
 }

