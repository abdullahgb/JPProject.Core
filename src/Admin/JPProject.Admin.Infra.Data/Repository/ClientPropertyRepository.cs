using IdentityServer4.EntityFramework.Entities;
using JPProject.Admin.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JPProject.Admin.Infra.Data.Repository
{
    public class ClientPropertyRepository : RepositoryDemo<ClientProperty>, IClientPropertyRepository
    {
        public ClientPropertyRepository(IConfigurationStore adminUiContext) : base(adminUiContext)
        {
        }

        public async Task<IEnumerable<ClientProperty>> GetByClientId(string clientId)
        {
            return await DbSet.Where(w => w.Client.ClientId == clientId).ToListAsync();
        }
    }
}