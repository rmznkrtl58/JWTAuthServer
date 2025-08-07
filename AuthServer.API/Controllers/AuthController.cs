using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateTokenByUser(UserLoginDto loginDto)
        {
            var result = await _authenticationService.CreateTokenAsync(loginDto);
            //aşağıdaki çağırdığım metod bidaha şart blogları yazıp status code ilgili koda eşitse başarılı veya başarısız dön demek yerine kısaltılmış şeklidir.
            return ActionResultInstance(result);
        }
        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var result=_authenticationService.CreateTokenByClient(clientLoginDto);
            return ActionResultInstance(result);
        }
        [HttpPost]
        public async Task<IActionResult> ResetRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result =await _authenticationService.ResetRefreshTokenAsync(refreshTokenDto.RefreshTokenCode);
            return ActionResultInstance(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result=await _authenticationService.CreateTokenByRefreshTokenAsync(refreshTokenDto.RefreshTokenCode);
            return ActionResultInstance(result);
        }
    }
}
