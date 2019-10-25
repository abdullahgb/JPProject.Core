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
    public class ClientSecretRepository : Repository<ClientSecret>, IClientSecretRepository
    {
        public ClientSecretRepository(IdentityServerContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ClientSecret>> GetByClientId(string clientId)
        {
            return await DbSet.Where(s => s.Client.ClientId == clientId).ToListAsync();
        }
    }
}