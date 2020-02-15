using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Admin.Infra.Data.Context;
using JPProject.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JPProject.Admin.Infra.Data.Repository
{
    public class IdentityResourceRepository : Repository<IdentityServer4.EntityFramework.Entities.IdentityResource>, IIdentityResourceRepository
    {
        public IdentityResourceRepository(IConfigurationDbStore adminUiContext) : base(adminUiContext)
        {
        }

        public Task<List<string>> SearchScopes(string search) => DbSet.Where(id => id.Name.Contains(search)).Select(x => x.Name).ToListAsync();
        public async Task<IEnumerable<IdentityResource>> All()
        {
            return  await DbSet.Select(s => s.ToModel()).ToListAsync();
        }

        public async Task<IdentityResource> GetByName(string name)
        {
            var idr = await DbSet.AsNoTracking().FirstOrDefaultAsync(w => w.Name == name);
            return idr.ToModel();
        }

        public async Task UpdateWithChildrens(string oldName, IdentityResource irs)
        {
            var entity = irs.ToEntity();
            var savedIR = await RemoveIdentityResourceClaimsAsync(oldName);
            entity.Id = savedIR.Id;
            Update(entity);
        }

        public async Task<IdentityResource> GetDetails(string name)
        {
            var ir = await DbSet.Include(s => s.UserClaims).AsNoTracking().FirstOrDefaultAsync(w => w.Name == name);
            return ir.ToModel();
        }

        private async Task<IdentityServer4.EntityFramework.Entities.IdentityResource> RemoveIdentityResourceClaimsAsync(string name)
        {
            var identityClaims = await DbSet.Include(s => s.UserClaims).Where(x => x.Name == name).AsNoTracking().FirstAsync();
            identityClaims.UserClaims.Clear();
            return identityClaims;
        }

        public void Add(IdentityResource obj)
        {
            base.Add(obj.ToEntity());
        }

        public void Update(IdentityResource obj)
        {
            var idr = DbSet.AsNoTracking().FirstOrDefault(w => w.Name == obj.Name);
            var newOne = obj.ToEntity();
            newOne.Id = idr.Id;
            base.Update(newOne);
        }

        public void Remove(IdentityResource obj)
        {
            var idr = DbSet.AsNoTracking().FirstOrDefault(w => w.Name == obj.Name);
            base.Remove(idr.Id);
        }
    }
}