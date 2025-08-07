using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;

namespace AuthServer.Core.Services
{
    public interface ITokenService
    {
        //Tokenlerimi üreteceğim service yapım
        TokenDto CreateTokenByUser(AppUser appUser);
        ClientTokenDto CreateTokenByClient(Client client);
    }
}
