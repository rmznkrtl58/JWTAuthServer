using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AuthServer.Core.Services;
using AuthServer.Service.MapProfile;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{   
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseDto<AppUserDto>> CreateUserAsync(CreateUserDto userDto)
        {
            var newUser = new AppUser()
            {
                UserName = userDto.Username,
                Email = userDto.MailAddress
            };
            var createResult = await _userManager.CreateAsync(newUser, userDto.Password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(x => x.Description).ToList();
                return ResponseDto<AppUserDto>.Fail(new ErrorDto(errors,true), 400);
            }
            var newUserDto = ObjectMapper.Mapper.Map<AppUserDto>(newUser);
            return ResponseDto<AppUserDto>.Success(newUserDto, 200);
        }

        public async Task<ResponseDto<AppUserDto>> GetUserByUsernameAsync(string userName)
        {
            var findUser = await _userManager.FindByNameAsync(userName);
            if (findUser is null) return ResponseDto<AppUserDto>.Fail("İlgili Kullanıcı Adı Bulunamadı",404,true);
            var newUserDto = ObjectMapper.Mapper.Map<AppUserDto>(findUser);
            return ResponseDto<AppUserDto>.Success(newUserDto, 200);
        }
    }
}
