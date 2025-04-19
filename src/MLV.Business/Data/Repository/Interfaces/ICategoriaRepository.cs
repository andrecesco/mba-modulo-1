using MLV.Business.Models;
using MLV.Core.Repository;

namespace MLV.Business.Data.Repository.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<Categoria> ObterPorId(Guid id);
    Task<Categoria> ObterPorNome(string nome);
    Task<List<Categoria>> ObterTodos();
    void Adicionar(Categoria model);
    void Atualizar(Categoria model);
    void Remover(Categoria model);
}
