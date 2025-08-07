using AutoMapper;
using System;

namespace AuthServer.Service.MapProfile
{
    public class ObjectMapper
    {
        //LazyLoading uygulama ilk ayağa kalktığında memorye gelmesin ben çağırdığım zaman memorye gelsin.
        //()->boş olarak görmem parametresiz çağırdığım için
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            //Auto Mapper tanımlaması
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMapper>();
            });
            return config.CreateMapper();
        });
        public static IMapper Mapper=> lazy.Value;
    }
}