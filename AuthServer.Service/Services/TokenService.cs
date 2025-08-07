using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Options;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;


namespace AuthServer.Service.Services
{   //Token oluşturmada Apilerimin haberi olmayacak bu token service classımı ilerde IAuthenticationServicim kullanacak
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly CustomTokenOption _customTokenOption;

        //IOption genericte geçiyorum CustomTokenOption classımı çünkü çünkü AppSettingsJsondaki değerleri mapladiğim içim
        public TokenService(UserManager<AppUser> userManager, IOptions<CustomTokenOption> option)
        {
            _userManager = userManager;
            _customTokenOption = option.Value;
        }
        private string CreateRefreshToken()
        {   //Random oluşan byte 32 bit türünde bir refresh tokenimi string türde oluşturdum.
            var numberByte=new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }
        //Aşağıdaki metodum tokenimin Payloadında bulunacak login olmuş kullanıcının bilgilerini tutacağımız claimleri barındıracak
        private IEnumerable<Claim> GetClaimByUser(AppUser appUser,List<string>audiences)
        {
            var userClaims = new List<Claim>()
            {
                //1.claim=>Kullanıcı Id'si bulunduracak
                new Claim(ClaimTypes.NameIdentifier,appUser.Id),
                //2.claim=>Kullanıcı Adı bulunduracak
                new Claim(ClaimTypes.Name,appUser.UserName),
                //3.claim=>Kullanıcı Emaili Bulundurucak
                new Claim(JwtRegisteredClaimNames.Email,appUser.Email),
                //4.claim=>Jwt Id'si için guid oluşturacak
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            //Kullanıcıya ait claimsler içerisine birden fazla value değeri ekleyeceğimden dolayı addRange ifadesi kullandım.
            //5.claim=>Aud->audience'den geliyor yani hangi apilerimin url'ine istek atacağım onu ekliyor claim olarak
            //Peki senaryo nasıl olacak=>apilerim tokeni doğrulamaya geçtiği zaman aud içerisinden kendisine gelen bir token olup olmadığını okuyarak isteğe olumlu yanıt verecek.
            userClaims.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return userClaims;
        }
        //Üyelik sistemi gerektirmeyen bir clientta ise claimlerimi aşağıdaki metod ile oluşturcam.
        private IEnumerable<Claim> GetClaimByClient(Client client)
        {
            var clientClaims = new List<Claim>()
            {
               //1.claim=>jwt Id için oluşturulan guid ifadesi
               new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
               //2.claim=>Tokenim kim için çalışacak
               new Claim(JwtRegisteredClaimNames.Sub,client.Id.ToString())
            };
            //Cliente ait claimsler içerisine birden fazla value değeri ekleyeceğimden dolayı addRange ifadesi kullandım.
            //3.claim=>Aud->audience'den geliyor yani hangi apilerimin url'ine istek atacağım onu ekliyor claim olarak
            clientClaims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return clientClaims;
        }
        public TokenDto CreateTokenByUser(AppUser appUser)
        {
            //AccessTokenimin zamanı
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);
            //RefreshTokenimin zamanı
            var refreshTokenExpiration=DateTime.Now.AddMinutes(_customTokenOption.RefreshTokenExpiration);
            //Tokenime ait imzamı oluşturcam
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOption.SecurityKey);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);
            //Tokenime ait özelliklerin atamasını yapacağım
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                 issuer: _customTokenOption.Issuer,
                 expires: accessTokenExpiration,
                 notBefore: DateTime.Now,
                 claims: GetClaimByUser(appUser, _customTokenOption.Audience),
                 signingCredentials: signingCredentials
                );
            //Tokenimi işleyecek,oluşturucak işlem
            var tokenHandler=new JwtSecurityTokenHandler();
            var accessToken= tokenHandler.WriteToken(jwtSecurityToken);
            //en son oluşacak tokenimi dönmem lazım
            var tokenDto = new TokenDto()
            {
                AccessToken=accessToken,
                RefreshToken=CreateRefreshToken(),
                AccessTokenExpiration=accessTokenExpiration,
                RefreshTokenExpiration=refreshTokenExpiration,
            };
            return tokenDto;
        }
        public ClientTokenDto CreateTokenByClient(Client client)
        {
            //AccessTokenimin zamanı
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);
            //Tokenime ait imzamı oluşturcam
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOption.SecurityKey);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            //Tokenime ait özelliklerin atamasını yapacağım
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                 issuer: _customTokenOption.Issuer,
                 expires: accessTokenExpiration,
                 notBefore: DateTime.Now,
                 claims: GetClaimByClient(client),
                 signingCredentials: signingCredentials
                );
            //Tokenimi işleyecek,oluşturucak işlem
            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.WriteToken(jwtSecurityToken);
            //en son oluşacak tokenimi dönmem lazım
            var clientTokenDto = new ClientTokenDto()
            {
                AccessToken = accessToken,
                AccessTokenExpiration = accessTokenExpiration,
            };
            return clientTokenDto;
        }
    }
}
