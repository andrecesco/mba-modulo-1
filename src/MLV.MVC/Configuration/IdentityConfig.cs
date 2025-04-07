using Microsoft.AspNetCore.Identity;
using MLV.Infra.Data;
using MLV.MVC.Data;
using MLV.MVC.Extensions;
using System.Text;

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
