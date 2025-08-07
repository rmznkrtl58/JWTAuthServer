using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.MapProfile
{
    public class DtoMapper:Profile
    {
        public DtoMapper()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<AppUser, AppUserDto>().ReverseMap();
        }
    }
}
