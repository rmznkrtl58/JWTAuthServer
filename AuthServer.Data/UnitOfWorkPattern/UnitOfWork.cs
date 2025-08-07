using AuthServer.Core.UnitOfWorkInterfaces;
using AuthServer.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AuthServer.Data.UnitOfWorkPattern
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public void Commit()
        {
            _context.SaveChanges();
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
