using AuthServer.Core.DTOs;
using SharedLibrary.DTOs;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{   //Direk olarak Apiye göndereceğim için responseDto dönüyorum ITokenService dış dünyaya açılmayıp sadece token ürettiğim yer olduğu için orada direk ilgili classlarımı dönüyorum
    public interface IAuthenticationService
    {
        Task<ResponseDto<TokenDto>> CreateTokenAsync(UserLoginDto loginDto);
        //refreshToken ile birlikte access token üretimi
        Task<ResponseDto<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken);
        //Kullanıcı logOut yaptığında refresh tokenı sıfırlama işlemi
        Task<ResponseDto<NoContentDto>> ResetRefreshTokenAsync(string 
            refreshToken);
        //ClientId-ClientSecret'lerimi apilerimin appSettingsJsonumda tutucam eğerki istek atmaya çalışan clientim tuttuğum Id-secretlerler eşleşiyorsa api isteği response çevirsin
        ResponseDto<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
}

