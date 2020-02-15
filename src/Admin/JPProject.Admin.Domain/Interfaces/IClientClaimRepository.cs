using IdentityServer4.EntityFramework.Entities;
using JPProject.Domain.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JPProject.Admin.Domain.Interfaces
{
    public interface IClientClaimRepository : IRepository<ClientClaim>
    {
        Task<IEnumerable<ClientClaim>> GetByClientId(string clientId);
    }
}