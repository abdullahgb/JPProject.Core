using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Domain.Core.Util;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jp.Database.Context;

namespace JPProject.Admin.EntityFramework.Repository.Repository
{
    public class IdentityResourceRepository : IIdentityResourceRepository
    {
        public DbSet<IdentityServer4.EntityFramework.Entities.IdentityResource> DbSet { get; set; }
        public IdentityResourceRepository(SsoContext context)
        {
            this.DbSet = context.IdentityResources;
        }
        //public IdentityResourceRepository(IConfigurationDbContext context)
        //{
        //    this.DbSet = context.IdentityResources;
        //}


        public Task<List<string>> SearchScopes(string search) => DbSet.Where(id => id.Name.Contains(search)).Select(x => x.Name).ToListAsync();
        public async Task<IEnumerable<IdentityResource>> All()
        {
            return await DbSet.Select(s => s.ToModel()).ToListAsync();
        }

        public Task<List<string>> ListIdentityResources()
        {
            return DbSet.Select(s => s.Name).ToListAsync();
        }

        public async Task<IdentityResource> GetByName(string name)
        {
            var idr = await DbSet.FirstOrDefaultAsync(w => w.Name == name);
            return idr.ToModel();
        }

        public async Task UpdateWithChildrens(string oldName, IdentityResource irs)
        {
            var entity = irs.ToEntity();
            var savedIr = await DbSet.Include(i => i.UserClaims).Where(x => x.Name == oldName).FirstAsync();
            entity.Id = savedIr.Id;
            entity.ShallowCopyTo(savedIr);
        }

        public async Task<IdentityResource> GetDetails(string name)
        {
            var ir = await DbSet.Include(s => s.UserClaims).FirstOrDefaultAsync(w => w.Name == name);
            return ir.ToModel();
        }

        private async Task<IdentityServer4.EntityFramework.Entities.IdentityResource> RemoveIdentityResourceClaimsAsync(string name)
        {
            var identityClaims = await DbSet.Include(s => s.UserClaims).Where(x => x.Name == name).FirstAsync();
            identityClaims.UserClaims.Clear();
            return identityClaims;
        }

        public void Add(IdentityResource obj)
        {
            DbSet.Add(obj.ToEntity());
        }

        public void Update(IdentityResource obj)
        {
            var idr = DbSet.SingleOrDefault(w => w.Name == obj.Name);
            var newOne = obj.ToEntity();
            newOne.ShallowCopyTo(idr);
        }

        public void Remove(IdentityResource obj)
        {
            var idr = DbSet.FirstOrDefault(w => w.Name == obj.Name);
            DbSet.Remove(idr);
        }

        public void Dispose()
        {
        }
    }
}