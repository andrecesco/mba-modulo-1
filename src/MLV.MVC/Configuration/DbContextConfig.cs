using MLV.MVC.Extensions;

namespace MLV.MVC.Configuration;

public static class DbContextConfig
{
    public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
    {
        builder.AddDbSelector();

        return builder;
    }
}
