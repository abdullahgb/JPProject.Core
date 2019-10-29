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
    public class ApiSecretRepository : Repository<ApiSecret>, IApiSecretRepository
    {
        public ApiSecretRepository(JpProjectContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ApiSecret>> GetByApiName(string name)
        {
            return await DbSet.Where(s => s.ApiResource.Name == name).ToListAsync();
        }
    }
}