using JPProject.Admin.Domain.Interfaces;
using JPProject.Admin.Infra.Data.Interfaces;
using JPProject.EntityFrameworkCore.Interfaces;
using System.Threading.Tasks;

namespace JPProject.Admin.Infra.Data.UoW
{
    public class UnitOfWork : IAdminUnitOfWork
    {
        private readonly IAdminContext _context;
        private readonly IEventStoreContext _eventStoreContext;

        public UnitOfWork(IAdminContext context, IEventStoreContext eventStoreContext)
        {
            _context = context;
            _eventStoreContext = eventStoreContext;
        }

        public async Task<bool> Commit()
        {
            var linesModified = await _context.SaveChangesAsync();
            await _eventStoreContext.SaveChangesAsync();
            return linesModified > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
            _eventStoreContext.Dispose();
        }
    }
}
