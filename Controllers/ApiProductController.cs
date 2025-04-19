using ASP_spr321.Data;
using ASP_spr321.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP_spr321.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ApiProductController(DataAccessor dataAccessor) : ControllerBase
    {
        private readonly DataAccessor _dataAcccessor = dataAccessor;

        [HttpGet("{id}")]
        public RestResponse Product(String id)
        {
            return new()
            {
                Service = "API Products ",
                DataType = "object",
                CacheTime = 600,
                Data = _dataAcccessor.GetProduct2(id),
            };
        }
    }
}
