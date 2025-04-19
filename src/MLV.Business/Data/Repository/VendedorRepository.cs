using Microsoft.EntityFrameworkCore;
using MLV.Business.Data.Repository.Interfaces;
using MLV.Business.Models;
using MLV.Core.Repository;

namespace MLV.Business.Data.Repository;

public class VendedorRepository(MlvDbContext context) : IVendedorRepository
{
    private bool _disposedValue;

    public IUnitOfWork UnitOfWork => context;

    public void Adicionar(Vendedor model)
    {
        context.Vendedores.Add(model);
    }

    public void Atualizar(Vendedor model)
    {
        context.Vendedores.Update(model);
    }

    public void Remover(Vendedor model)
    {
        context.Vendedores.Remove(model);
    }

    public async Task<List<Vendedor>> ObterTodos()
    {
        return await context.Vendedores
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Vendedor> ObterPorId(Guid id)
    {
        return await context.Vendedores
            .FindAsync(id);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                context.Dispose();
            }

            _disposedValue = true;
        }
    }
}
