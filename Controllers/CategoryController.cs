using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductAPI.Controllers
{
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        
    }
}
