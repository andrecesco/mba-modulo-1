namespace MLV.Core.Repository;

public interface IUnitOfWork
{
    Task<bool> Commit();
}
