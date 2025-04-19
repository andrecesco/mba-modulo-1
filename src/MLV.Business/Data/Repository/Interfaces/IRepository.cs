using MLV.Business.Data;

namespace MLV.Core.Repository;

public interface IRepository<T> : IDisposable where T : Entity
{
    IUnitOfWork UnitOfWork { get; }
}
