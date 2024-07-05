using Bogus;
using Domain.Entities;
using Utils.PasswordEncryptor;

namespace Utils.Entities;

public class UserBuilder
{
    public static (User user, string password) Build()
    {
        var password = string.Empty;

        var userCreated = new Faker<User>()
            .RuleFor(x => x.Id, _ => 1)
            .RuleFor(x => x.Name,f => f.Person.FullName)
            .RuleFor(x => x.Email,f => f.Person.Email)
            .RuleFor(x => x.YearBorn, 18.ToString)
			.RuleFor(x => x.Password, f =>
            {
                password = f.Internet.Password();

                return PasswordEncryptorBuilder.Instance().Encrypt(password);
            });

        return (userCreated, password);
    }
}