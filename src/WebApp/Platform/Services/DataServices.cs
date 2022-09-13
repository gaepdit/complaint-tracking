﻿using Cts.Domain.ActionTypes;
using Cts.Domain.Offices;
using Cts.Infrastructure.Contexts;
using Cts.Infrastructure.Repositories;
using Cts.LocalRepository.Repositories;
using Cts.WebApp.Platform.Settings;
using Microsoft.EntityFrameworkCore;

namespace Cts.WebApp.Platform.Services;

public static class DataServices
{
    public static void AddDataServices(this IServiceCollection services,
        ConfigurationManager configuration,
        bool isLocal)
    {
        // When running locally, you have the option to use in-memory data or build the database using LocalDB.
        if (isLocal && !ApplicationSettings.LocalDevSettings.BuildLocalDb)
        {
            services.AddDbContext<CtsDbContext>(opts =>
                opts.UseInMemoryDatabase("CTS_DB"));

            // Uses static data if no database is built.
            services.AddSingleton<IActionTypeRepository, LocalActionTypeRepository>();
            services.AddSingleton<IOfficeRepository, LocalOfficeRepository>();
        }
        else
        {
            services.AddDbContext<CtsDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("Infrastructure")));

            services.AddScoped<IActionTypeRepository, ActionTypeRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
        }
    }
}
