

using AuthServer.Core.DTOs;
using SharedLibrary.DTOs;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IUserService
    {
        Task<ResponseDto<AppUserDto>> CreateUserAsync(CreateUserDto userDto);
        Task<ResponseDto<AppUserDto>> GetUserByUsernameAsync(string userName);
    }
}
