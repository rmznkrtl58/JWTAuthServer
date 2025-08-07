using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
   public interface IGenericService<T,TDto>where T:class where TDto : class
    {
        //listeleme
        Task<ResponseDto<IEnumerable<TDto>>> TGetListAllAsync();
        //Id'ye göre getirme
        Task<ResponseDto<TDto>> TGetByIdAsync(int id);
        //Şarta göre getirme
        Task<ResponseDto<IEnumerable<TDto>>> TGetByFilter(Expression<Func<T,bool>>filter);
        //Ekleme
        Task<ResponseDto<TDto>> TCreateAsync(TDto tdto);
        //Silme
        Task<ResponseDto<NoContentDto>> TDeleteAsync(int id);
        //Güncelleme
        Task<ResponseDto<NoContentDto>> TUpdateAsync(TDto tdto, int id);
    }
}
