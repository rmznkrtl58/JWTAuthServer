using System;


namespace AuthServer.Core.Entities
{
    public class UserRefreshToken
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        //refresh Tokenin Kullanılma Süresi
        public DateTime Expiration { get; set; }
    }
}
