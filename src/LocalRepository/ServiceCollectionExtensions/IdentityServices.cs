﻿using Cts.AppServices.StaffServices;
using Cts.Domain.Entities;
using Cts.LocalRepository.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Cts.LocalRepository.ServiceCollectionExtensions;

public static class IdentityServices
{
    public static void AddLocalIdentity(this IServiceCollection services)
    {
        services.AddSingleton<IUserStore<ApplicationUser>, LocalUserStore>();
        services.AddSingleton<IRoleStore<IdentityRole>, LocalRoleStore>();
        services.AddTransient<IStaffAppService, LocalStaffAppService>();
    }
}
