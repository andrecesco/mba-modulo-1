using MLV.Business.Data;
using MLV.Business.Data.Repository;
using MLV.Business.Data.Repository.Interfaces;
using MLV.Business.Services;
using MLV.Business.Services.Interfaces;

namespace MLV.ApiRest.Configuration;

public static class DependencyInjectionConfig
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<MlvDbContext>();

        builder.Services.AddScoped<IVendedorRepository, VendedorRepository>();
        builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

        builder.Services.AddScoped<IVendedorService, VendedorService>();
        builder.Services.AddScoped<IProdutoService, ProdutoService>();
        builder.Services.AddScoped<ICategoriaService, CategoriaService>();

        return builder;
    }
}
