using ASP_spr321.Data;
using ASP_spr321.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP_spr321.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class ApiCategoryController(DataAccessor dataAccessor) : ControllerBase
    {
        private readonly DataAccessor _dataAcccessor = dataAccessor;
        [HttpGet]
        public RestResponse Categories()
        {
            return new() 
            { 
                Service="API Products Categories",
                DataType="array",
                CacheTime=600,
                Data = _dataAcccessor.AllCategories(),
            };
        }
        [HttpGet("{id}")]
        public RestResponse Category(String id)
        {
            return new()
            {
                Service = "API Products Categories",
                DataType = "object",
                CacheTime = 600,
                Data = _dataAcccessor.GetCategory(id),
            };
        }
    }
}
