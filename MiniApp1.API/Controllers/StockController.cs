using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace MiniApp1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {  
        //Controllerimin ismi önemli değil öğrenmek için rastgele isimlendirdik maksat koruma altına alabiliyor muyuz onu kontrol için
        [Authorize]
        [HttpGet]
        public IActionResult GetStockByUser()
        {
            var userName = User.Identity.Name;
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var nameAndUserId = @$"(Stock)=>Kimliği yetkilenmiş kişinin Kullanıcı Adı:{userName} Ve Id'si:{userId}";
            return Ok(nameAndUserId);
        }
    }
}
