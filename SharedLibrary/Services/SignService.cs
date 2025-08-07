using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SharedLibrary.Services
{
    public static class SignService
    {   //Bizim imzamımızı gerçekleştireceğimiz yapıdır.
        public static SecurityKey GetSymmetricSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
