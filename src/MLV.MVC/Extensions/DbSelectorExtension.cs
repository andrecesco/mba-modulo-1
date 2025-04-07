using Microsoft.EntityFrameworkCore;
using MLV.Infra.Data;

namespace MLV.MVC.Extensions;

public static class DbSelectorExtension
{
    public static void AddDbSelector(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddDbContext<MlvDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLite"));
                //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }
        else
        {
            builder.Services.AddDbContext<MlvDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}
