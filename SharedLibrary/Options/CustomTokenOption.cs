

using System.Collections.Generic;
using System;

namespace SharedLibrary.Options
{   //Apilerimizin->AppsettingsJsonunda token bilgilerini tanımlamıştık onlara karşılık gelen değerleri burdaki classımla mapleyeceğim
    public class CustomTokenOption
    {
        //birden fazla Apime istek atabileceğimden dolayı list türünde tutuyorum
        public List<String> Audience { get; set; }
        //Tokenimi dağıtacak tek bir dağıtıcı olacak
        public string Issuer { get; set; }
        //Access Tokenimin Ömür süresi
        public int AccessTokenExpiration { get; set; }
        //Refresh Tokenimin Ömür süresi
        public int RefreshTokenExpiration { get; set; }
        //Access Tokenime simetrik olarak imzalacağım keyim.
        public string SecurityKey { get; set; }
    }
}
