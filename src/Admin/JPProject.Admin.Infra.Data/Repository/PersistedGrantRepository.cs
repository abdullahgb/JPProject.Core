using IdentityServer4.EntityFramework.Entities;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Domain.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JPProject.Admin.Infra.Data.Repository
{
    public class PersistedGrantRepository : RepositoryDemo<PersistedGrant>, IPersistedGrantRepository
    {
        public PersistedGrantRepository(IConfigurationStore adminUiContext) : base(adminUiContext)
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