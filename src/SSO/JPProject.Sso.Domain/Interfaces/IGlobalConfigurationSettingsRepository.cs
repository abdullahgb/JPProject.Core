﻿using System.Linq;
using System.Threading.Tasks;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Domain.Models;

namespace JPProject.Sso.Domain.Interfaces
{
    public interface IGlobalConfigurationSettingsRepository : IRepository<GlobalConfigurationSettings>
    {
        Task<GlobalConfigurationSettings> FindByKey(string key);
        IQueryable<GlobalConfigurationSettings> GetAll();
    }
}
