using IdentityServer4.EntityFramework.Entities;
using JPProject.Domain.Core.ViewModels;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Admin.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JPProject.EntityFrameworkCore.Context;

namespace JPProject.Admin.Infra.Data.Repository
{
    public class PersistedGrantRepository : Repository<PersistedGrant>, IPersistedGrantRepository
    {
        public PersistedGrantRepository(IdentityServerContext context) : base(context)
        {
        }

        public Task<List<PersistedGrant>> GetGrants(PagingViewModel paging)
        {
            return DbSet.AsNoTracking().OrderByDescending(s => s.CreationTime).Skip(paging.Offset).Take(paging.Limit).ToListAsync();
        }

        public Task<int> Count()
        {
            return DbSet.CountAsync();
        }
    }
}