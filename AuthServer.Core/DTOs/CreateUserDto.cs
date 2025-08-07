

namespace AuthServer.Core.DTOs
{
    public class CreateUserDto
    {//kullanıcı ilk kayıt olurken sıkmamak lazım gerektiğinde başka bilgileri varsa ayarlar kısmından güncelleyecektir
        public string Username { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
    }
}
