using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Admin.Infra.Data.Context;
using JPProject.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;

namespace JPProject.Admin.Infra.Data.Repository
{
    public class ClientPropertyRepository : Repository<ClientProperty>, IClientPropertyRepository
    {
        public ClientPropertyRepository(IdentityServerContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ClientProperty>> GetByClientId(string clientId)
        {
            return await DbSet.Where(w => w.Client.ClientId == clientId).ToListAsync();
        }
    }
}