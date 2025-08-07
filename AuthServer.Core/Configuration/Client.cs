using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Configuration
{   //AuthServerime istek yapacak bir mobil uygulama veya web uygulamasıda olabilir
    public class Client//Kendi iç projemizde kullanacağımızdan dolayı Dto demedim dış dünyaya açsaydık derdim
    {
        public string Id { get; set; }
        //Clientlerımın sahip olduğu şifre
        public string Secret { get; set; }
        //Clientlerimin hangi Apilere erişim sağlayacağını tutacağım propum
        public List<string> Audiences { get; set; }

    }
}
