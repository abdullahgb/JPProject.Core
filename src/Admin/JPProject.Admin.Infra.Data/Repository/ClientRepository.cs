using IdentityServer4.EntityFramework.Entities;
using JPProject.Admin.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JPProject.Admin.Infra.Data.Repository
{
    public class ClientRepository : RepositoryDemo<Client>, IClientRepository
    {
        public ClientRepository(IConfigurationStore adminUiContext) : base(adminUiContext)
        {
        }

        public Task<Client> GetClient(string clientId)
        {
            return DbSet
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.Properties)
                .AsNoTracking()
                .Where(x => x.ClientId == clientId)
                .SingleOrDefaultAsync();
        }

        public Task<Client> GetByClientId(string clientId)
        {
            return DbSet.Where(x => x.ClientId == clientId).SingleOrDefaultAsync();
        }

        public Task<Client> GetClientDefaultDetails(string clientId)
        {
            return DbSet
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .AsNoTracking()
                .Where(x => x.ClientId == clientId)
                .SingleOrDefaultAsync();
        }

        public async Task UpdateWithChildrens(Client client)
        {
            await RemoveClientRelationsAsync(client);
            Update(client);
        }


        private async Task RemoveClientRelationsAsync(Client client)
        {
            var clientDb = await DbSet
                                .Include(s => s.AllowedScopes)
                                .Include(s => s.AllowedCorsOrigins)
                                .Include(s => s.AllowedGrantTypes)
                                .Include(s => s.RedirectUris)
                                .Include(s => s.AllowedCorsOrigins)
                                .Include(s => s.IdentityProviderRestrictions)
                                .Include(s => s.PostLogoutRedirectUris)
                                .Where(x => x.Id == client.Id).AsNoTracking().FirstAsync();

            clientDb.AllowedScopes?.Clear();
            clientDb.AllowedGrantTypes?.Clear();
            clientDb.RedirectUris?.Clear();
            clientDb.AllowedCorsOrigins?.Clear();
            clientDb.IdentityProviderRestrictions?.Clear();
            clientDb.PostLogoutRedirectUris?.Clear();
        }

    }
}
