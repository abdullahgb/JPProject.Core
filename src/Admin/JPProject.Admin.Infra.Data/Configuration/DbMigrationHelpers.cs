﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using JPProject.Admin.Infra.Data.Context;
using JPProject.Domain.Core.Events;
using JPProject.Domain.Core.Exceptions;
using JPProject.EntityFrameworkCore.MigrationHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Admin.Infra.Data.Configuration
{
    public static class DbMigrationHelpers
    {
        public static async Task CheckDatabases(IServiceProvider serviceProvider, JpDatabaseOptions options)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var id4Context = scope.ServiceProvider.GetRequiredService<JPProjectAdminUIContext>();

            if (id4Context.Database.IsInMemory())
                return;

            await DbHealthChecker.TestConnection(id4Context);
            await ValidateIs4Context(options, id4Context);


        }

        private static async Task ValidateIs4Context(JpDatabaseOptions options, JPProjectAdminUIContext id4AdminUiContext)
        {
            var configurationDatabaseExist = await DbHealthChecker.CheckTableExists<Client>(id4AdminUiContext);
            var operationalDatabaseExist = await DbHealthChecker.CheckTableExists<PersistedGrant>(id4AdminUiContext);
            var isDatabaseExist = configurationDatabaseExist && operationalDatabaseExist;

            if (!isDatabaseExist && options.MustThrowExceptionIfDatabaseDontExist)
                throw new DatabaseNotFoundException("IdentityServer4 Database doesn't exist. Ensure it was created before.'");
        }
    }
}
