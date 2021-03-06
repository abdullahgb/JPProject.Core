using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Domain.Core.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jp.Database.Context;
using static JPProject.Admin.EntityFramework.Repository.Repository.ModelMappers;
using ApiResource = IdentityServer4.Models.ApiResource;
using Secret = IdentityServer4.Models.Secret;

namespace JPProject.Admin.EntityFramework.Repository.Repository
{
    public class ApiResourceRepository : IApiResourceRepository
    {
        public DbSet<IdentityServer4.EntityFramework.Entities.ApiResource> DbSet { get; set; }
        public ApiResourceRepository(SsoContext context)
        {
            DbSet = context.ApiResources;
        }
        //public ApiResourceRepository(IConfigurationDbContext context)
        //{
        //    DbSet = context.ApiResources;
        //}

        private ApiResource ToModel(IdentityServer4.EntityFramework.Entities.ApiResource arg)
        {
            return arg.ToModel();
        }

        public async Task<List<ApiResource>> GetResources()
        {
            var apiResource = await DbSet.Include(s => s.UserClaims).ToListAsync();
            return apiResource.Select(ToModel).ToList();
        }


        public async Task<ApiResource> GetResource(string name)
        {
            var apiResource = await DbSet
                .Include(s => s.Secrets)
                .Include(s => s.Scopes)
                .Include(s => s.Properties)
                .Include(s => s.UserClaims)
                                    .FirstOrDefaultAsync(s => s.Name == name);

            return apiResource.ToModel();
        }

        public void RemoveSecret(string resourceName, Secret secret)
        {
            var client = GetResourceByName(resourceName);
            client.Secrets.RemoveAll(r => r.Type == secret.Type && r.Value == secret.Value);

            DbSet.Update(client);
        }

        public void AddSecret(string resourceName, Secret secret)
        {
            var apiResource = GetResourceByName(resourceName);
            var entity = Mapper.Map<ApiSecret>(secret);
            apiResource.Secrets.Add(entity);
        }

        public void RemoveScope(string resourceName, string name)
        {
            var apiResource = GetResourceByName(resourceName);
            apiResource.Scopes.RemoveAll(r => r.Name == name);
        }

        private IdentityServer4.EntityFramework.Entities.ApiResource GetResourceByName(string resourceName)
        {
            var apiResource = DbSet
                                .Include(s => s.Secrets)
                                .Include(s => s.Scopes)
                                .Include(s => s.Properties)
                                .Include(s => s.UserClaims)
                                .FirstOrDefault(s => s.Name == resourceName);
            return apiResource;
        }

        public void AddScope(string resourceName, Scope scope)
        {
            var apiResource = GetResourceByName(resourceName);
            var entity = Mapper.Map<ApiScope>(scope);
            apiResource.Scopes.Add(entity);
        }

        public async Task<IEnumerable<Secret>> GetSecretsByApiName(string name)
        {
            var secrets = await DbSet.Where(w => w.Name == name).SelectMany(s => s.Secrets).ToListAsync();
            return Mapper.Map<IEnumerable<Secret>>(secrets);
        }


        public async Task UpdateWithChildrens(string oldName, ApiResource irs)
        {
            var apiDb = await DbSet
                .Include(s => s.UserClaims)
                .Where(x => x.Name == oldName).FirstAsync();
            var newIr = irs.ToEntity();

            newIr.Id = apiDb.Id;
            newIr.ShallowCopyTo(apiDb);
        }

        public void Add(ApiResource obj)
        {
            DbSet.Add(obj.ToEntity());
        }

        public void Update(ApiResource obj)
        {
            var apiResource = DbSet.FirstOrDefault(w => w.Name == obj.Name);
            var newOne = obj.ToEntity();
            newOne.Id = apiResource.Id;
            DbSet.Update(newOne);
        }

        public void Remove(ApiResource obj)
        {
            var apiResource = DbSet.FirstOrDefault(w => w.Name == obj.Name);
            DbSet.Remove(apiResource);
        }

        public async Task<IEnumerable<Secret>> GetByApiName(string name)
        {
            var api = await GetResource(name);
            return api.ApiSecrets;
        }

        public async Task<List<Scope>> SearchScopes(string search)
        {
            var scopes = await DbSet.Where(id => id.Name.Contains(search)).ToListAsync();
            return Mapper.Map<List<Scope>>(scopes);
        }

        public async Task<IEnumerable<Scope>> GetScopesByResource(string search)
        {
            var scopes = await DbSet.Include(s => s.Scopes).Where(id => id.Name == search).SelectMany(s => s.Scopes).ToListAsync();

            return Mapper.Map<List<Scope>>(scopes);
        }

        public Task<List<string>> ListResources()
        {
            return DbSet.Select(s => s.Name).ToListAsync();
        }

        public void Dispose()
        {

            GC.SuppressFinalize(this);
        }
    }
}