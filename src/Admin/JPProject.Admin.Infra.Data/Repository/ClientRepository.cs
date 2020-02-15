using FluentValidation;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Admin.Infra.Data.Context;
using JPProject.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static JPProject.Admin.Infra.Data.Repository.ModelMappers;

namespace JPProject.Admin.Infra.Data.Repository
{
    public class ClientRepository : Repository<IdentityServer4.EntityFramework.Entities.Client>, IClientRepository
    {
        public ClientRepository(IConfigurationDbStore adminUiContext) : base(adminUiContext)
        {
        }

        public async Task<Client> GetClient(string clientId)
        {
            var client = await DbSet
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

            return client.ToModel();
        }

        public async Task<Client> GetByClientId(string clientId)
        {
            var client = await DbSet.Where(x => x.ClientId == clientId).SingleOrDefaultAsync();
            return client.ToModel();
        }

        public async Task<Client> GetClientDefaultDetails(string clientId)
        {
            var client = await DbSet
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .AsNoTracking()
                .Where(x => x.ClientId == clientId)
                .SingleOrDefaultAsync();
            return client.ToModel();
        }

        public void RemoveSecret(string clientId, Secret secret)
        {
            var client = DbSet.Include(s => s.ClientSecrets).AsNoTracking().FirstOrDefault(s => s.ClientId == clientId);
            client.ClientSecrets.RemoveAll(r => r.Type == secret.Type && r.Value == secret.Value);

            DbSet.Update(client);
        }

        public async Task AddSecret(string clientId, Secret secret)
        {
            var client = await FindClient(clientId);
            var entity = Mapper.Map<IdentityServer4.EntityFramework.Entities.ClientSecret>(secret);
            client.ClientSecrets.Add(entity);
            DbSet.Update(client);

        }

        public async Task RemoveProperty(string clientId, string key, string value)
        {
            var client = await FindClient(clientId);
            client.Properties.RemoveAll(r => r.Key == key && r.Value == value);
            DbSet.Update(client);
        }

        public async Task AddProperty(string clientId, string key, string value)
        {
            var client = await FindClient(clientId);
            client.Properties.Add(new IdentityServer4.EntityFramework.Entities.ClientProperty() { Key = key, Value = value });
            DbSet.Update(client);
        }

        public async Task RemoveClaim(string clientId, string type)
        {
            var client = await FindClient(clientId);
            client.Properties.RemoveAll(r => r.Key == type);
            DbSet.Update(client);
        }

        public async Task RemoveClaim(string clientId, string type, string value)
        {

            var client = await FindClient(clientId);
            client.Properties.RemoveAll(r => r.Key == type && r.Value == value);
            DbSet.Update(client);
        }

        public void AddClaim(string clientId, Claim claim)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Secret>> GetSecrets(string clientId)
        {
            var client = await FindClient(clientId);
            return Mapper.Map<IEnumerable<Secret>>(client.ClientSecrets);
        }

        public async Task<IEnumerable<Client>> All()
        {
            var clients = await DbSet.OrderBy(a => a.ClientName).ToListAsync();
            return clients.Select(s => s.ToModel());
        }

        public async Task<IDictionary<string, string>> GetProperties(string clientId)
        {
            var client = await FindClient(clientId);
            return Mapper.Map<IDictionary<string, string>>(client.Properties);
        }

        public async Task<IEnumerable<Claim>> GetClaims(string clientId)
        {
            var client = await FindClient(clientId);
            return Mapper.Map<IEnumerable<Claim>>(client.Claims);
        }

        private async Task<IdentityServer4.EntityFramework.Entities.Client> FindClient(string clientId)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(f => f.ClientId == clientId);
        }

        public async Task UpdateWithChildrens(string oldClientId, Client client)
        {
            var newClient = client.ToEntity();
            var clientDb = await RemoveClientRelationsAsync(oldClientId);

            newClient.Id = clientDb.Id;
            Update(newClient);
        }


        private async Task<IdentityServer4.EntityFramework.Entities.Client> RemoveClientRelationsAsync(string clientId)
        {
            var clientDb = await DbSet
                                .Include(s => s.AllowedScopes)
                                .Include(s => s.AllowedCorsOrigins)
                                .Include(s => s.AllowedGrantTypes)
                                .Include(s => s.RedirectUris)
                                .Include(s => s.AllowedCorsOrigins)
                                .Include(s => s.IdentityProviderRestrictions)
                                .Include(s => s.PostLogoutRedirectUris)
                                .Where(x => x.ClientId == clientId).AsNoTracking().FirstAsync();

            clientDb.AllowedScopes?.Clear();
            clientDb.AllowedGrantTypes?.Clear();
            clientDb.RedirectUris?.Clear();
            clientDb.AllowedCorsOrigins?.Clear();
            clientDb.IdentityProviderRestrictions?.Clear();
            clientDb.PostLogoutRedirectUris?.Clear();
            return clientDb;
        }

        public void Add(Client obj)
        {
            base.Add(obj.ToEntity());
        }

        public void Update(Client obj)
        {
            var client = DbSet.AsNoTracking().FirstOrDefault(w => w.ClientId == obj.ClientId);
            var newOne = obj.ToEntity();
            newOne.Id = client.Id;
            base.Update(newOne);
        }

        public void Remove(Client obj)
        {
            var client = DbSet.AsNoTracking().FirstOrDefault(w => w.ClientId == obj.ClientId);
            base.Remove(client);
        }


    }
}
