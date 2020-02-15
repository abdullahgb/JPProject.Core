using IdentityServer4.Models;
using JPProject.Domain.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JPProject.Admin.Domain.Interfaces
{
    public interface IPersistedGrantRepository
    {
        Task<List<PersistedGrant>> GetGrants(PagingViewModel paging);
        Task<int> Count();
        Task<PersistedGrant> GetGrant(string key);
        void Remove(PersistedGrant grant);
    }
}