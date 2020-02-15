using IdentityServer4.EntityFramework.Entities;
using JPProject.Admin.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JPProject.Admin.Infra.Data.Repository
{
    public class ApiSecretRepository : RepositoryDemo<ApiSecret>, IApiSecretRepository
    {
        public ApiSecretRepository(IConfigurationStore adminUiContext) : base(adminUiContext)
        {
        }

        public async Task<IEnumerable<ApiSecret>> GetByApiName(string name)
        {
            return await DbSet.Where(s => s.ApiResource.Name == name).ToListAsync();
        }
    }
}