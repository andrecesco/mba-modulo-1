using FluentValidation.Results;
using MediatR;
using MLV.Business.Commands;
using MLV.Business.Handlers;
using MLV.Business.Interfaces;
using MLV.Core.Mediator;
using MLV.Infra.Data;
using MLV.Infra.Data.Repository;
using System.Reflection;

namespace MLV.MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        builder.Services.AddScoped<MlvDbContext>();

        builder.Services.AddScoped<IVendedorRepository, VendedorRepository>();
        builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

        builder.RegisterCommands();

        return builder;
    }

    public static WebApplicationBuilder RegisterCommands(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
        builder.Services.AddScoped<IRequestHandler<VendedorCommand, ValidationResult>, VendedorHandler>();
        builder.Services.AddScoped<IRequestHandler<VendedorAtualizarCommand, ValidationResult>, VendedorHandler>();

        builder.Services.AddScoped<IRequestHandler<ProdutoCriarCommand, ValidationResult>, ProdutoHandler>();
        builder.Services.AddScoped<IRequestHandler<ProdutoAtualizarCommand, ValidationResult>, ProdutoHandler>();
        builder.Services.AddScoped<IRequestHandler<ProdutoRemoverCommand, ValidationResult>, ProdutoHandler>();

        builder.Services.AddScoped<IRequestHandler<CategoriaCriarCommand, ValidationResult>, CategoriaHandler>();
        builder.Services.AddScoped<IRequestHandler<CategoriaAtualizarCommand, ValidationResult>, CategoriaHandler>();
        builder.Services.AddScoped<IRequestHandler<CategoriaRemoverCommand, ValidationResult>, CategoriaHandler>();
        return builder;
    }
}
