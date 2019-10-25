using JPProject.Domain.Core.Interfaces;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Admin.Infra.Data.Context;
using JPProject.EntityFrameworkCore.Context;

namespace JPProject.Admin.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityServerContext _context;

        public UnitOfWork(IdentityServerContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
