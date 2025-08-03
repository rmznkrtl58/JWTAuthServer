using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthServer.Core.Repositories
{
   public interface IGenericRepository<T>where T:class
    {
        //Listeleme
        IQueryable<T> GetListAll();
        //Belli Bir Şarta Göre Getirme
        IQueryable<T> GetByFilter(Expression<Func<T, bool>> filter);
        //Id'ye Göre Getirme
        Task<T> GetByIdAsync(int id);
        //Ekleme
        Task CreateAsync(T t);
        //Silme
        void Delete(int id);
        //Güncelleme
        T Update(T t);
       
    }
}
