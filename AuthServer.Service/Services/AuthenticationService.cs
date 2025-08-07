using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWorkInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        //Clientlar birden fazla olabilir appsettingsJsonda bir spa birde mobil uygulama için tanımlamalar yaptım oyuzden constructorda geçerken list ve IOption generic yapıda geçmek gerek.
        private readonly List<Client> _clients;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(IOptions<List<Client>> optionClients, UserManager<AppUser> userManager, IGenericRepository<UserRefreshToken> userRefreshTokenService, ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            this._clients = optionClients.Value;
            _userManager = userManager;
            _userRefreshTokenService = userRefreshTokenService;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<TokenDto>> CreateTokenAsync(UserLoginDto loginDto)
        {
            if(loginDto is null) throw new ArgumentNullException(nameof(loginDto));
            var findUser = await _userManager.FindByEmailAsync(loginDto.Email);
            if (findUser is null) return ResponseDto<TokenDto>.Fail("Böyle bir kullanıcı Bulunamadı!", 400, true);
            var checkPassword = await _userManager.CheckPasswordAsync(findUser, loginDto.Password);
            if (!checkPassword) return ResponseDto<TokenDto>.Fail("Email Veya Şifre Hatalı!", 400, true);
            var token = _tokenService.CreateTokenByUser(findUser);
            //ilgili kullanıcıya ait refresh token mevcut mu?
            var userRefreshToken = await _userRefreshTokenService.GetByFilter(x => x.UserId == findUser.Id).SingleOrDefaultAsync();
            if(userRefreshToken is null)//eğer ilgili kullanıcının refresh tokeni yoksa
            {
                var newUserRefreshToken = new UserRefreshToken()
                {
                    Code = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration,
                    UserId = findUser.Id,
                };
                await _userRefreshTokenService.CreateAsync(newUserRefreshToken);
            }
            else//var ise güncelleme yapılır
            {
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
                userRefreshToken.Code = token.RefreshToken;
            }
            await _unitOfWork.CommitAsync();
            return ResponseDto<TokenDto>.Success(token, 200);
        }
        public ResponseDto<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var findClient = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId&&x.Secret==clientLoginDto.ClientSecret);
            if (findClient is null) return ResponseDto<ClientTokenDto>.Fail("İlgili Client Bulunamadı", 404, true);
            var token=_tokenService.CreateTokenByClient(findClient);
            return ResponseDto<ClientTokenDto>.Success(token, 200);
        }

        public async Task<ResponseDto<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var findRefreshToken = await _userRefreshTokenService.GetByFilter(c => c.Code == refreshToken).SingleOrDefaultAsync();
            if (findRefreshToken is null) return ResponseDto<TokenDto>.Fail("İlgili refresh Token Bulunamadı", 404, true);
            var findUser = await _userManager.FindByIdAsync(findRefreshToken.UserId);
            if(findUser is null) return ResponseDto<TokenDto>.Fail("İlgili Kullanıcı Bulunamadı", 404, true);
            var token=_tokenService.CreateTokenByUser(findUser);
            findRefreshToken.Code = token.RefreshToken;
            findRefreshToken.Expiration = token.RefreshTokenExpiration;
            await _unitOfWork.CommitAsync();
            return ResponseDto<TokenDto>.Success(token, 200);
        }

        public async Task<ResponseDto<NoContentDto>> ResetRefreshTokenAsync(string refreshToken)
        {
            var findRefreshToken =await _userRefreshTokenService.GetByFilter(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (findRefreshToken is null) return ResponseDto<NoContentDto>.Fail("İlgili Refresh Token Bulunamadı",404,true);
            _userRefreshTokenService.Delete(findRefreshToken);
            await _unitOfWork.CommitAsync();
            return ResponseDto<NoContentDto>.Success(200);
        }
    }
}
