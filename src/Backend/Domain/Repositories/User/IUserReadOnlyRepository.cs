namespace Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
    Task<bool> ExistsByEmail(string email);
    Task<Entities.User> Login (string email, string password);
    Task<Entities.User> GetByEmail(string email);
}