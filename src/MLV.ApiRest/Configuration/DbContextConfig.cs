using MLV.ApiRest.Extensions;

namespace MLV.ApiRest.Configuration;

public static class DbContextConfig
{
    public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
    {
        builder.AddDbSelector();

        return builder;
    }
}