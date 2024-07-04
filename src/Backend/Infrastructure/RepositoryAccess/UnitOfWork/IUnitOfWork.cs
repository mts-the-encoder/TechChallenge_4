namespace Infrastructure.RepositoryAccess.UnitOfWork;

public interface IUnitOfWork
{
    Task Commit();
}