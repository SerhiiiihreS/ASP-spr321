using ASP_spr321.Data;
using ASP_spr321.Models;
using ASP_spr321.Services.Kdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using ASP_spr321.Services.Storage;
using Microsoft.AspNetCore.Authentication;

namespace ASP_spr321.Controllers
{
    
    [Route("api/user")]
    [ApiController]
    public class ApiUserController(DataAccessor dataAccessor ): ControllerBase
    {
        private readonly DataAccessor _dataAccessor = dataAccessor; 

        [HttpGet]
        public RestResponse Authenticate()
        {
            var res= new RestResponse
            {
                Service = "API User Authentication",
                DataType = "object",
                CacheTime = 600,
            };
            try
            {
                res.Data = _dataAccessor.Authenticate(Request);
            }
            catch (Win32Exception ex)
            {
                res.Status=new()
                {
                    IsOk = false,
                    Code = ex.ErrorCode,
                    Phrase = ex.Message
                };
                res.Data = null;
            }
            return res;
        }
    }
}
