using Microsoft.AspNetCore.Identity;
using MLV.Business.Data;
using MLV.MVC.Extensions;

namespace MLV.MVC.Configuration;

public static class IdentityConfig
{
    public static WebApplicationBuilder AddIdentityConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<MlvDbContext>()
            .AddErrorDescriber<IdentityMensagensPortugues>();

        builder.Services.AddHttpContextAccessor();

        return builder;
    }
}
