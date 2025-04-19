using MLV.Business.Models;
using MLV.Core.Repository;

namespace MLV.Business.Data.Repository.Interfaces;

public interface IVendedorRepository : IRepository<Vendedor>
{
    Task<Vendedor> ObterPorId(Guid id);
    Task<List<Vendedor>> ObterTodos();
    void Adicionar(Vendedor model);
    void Atualizar(Vendedor model);
    void Remover(Vendedor model);
}
