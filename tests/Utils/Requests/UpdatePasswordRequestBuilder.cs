using Bogus;
using Communication.Requests;

namespace Utils.Requests;

public class UpdatePasswordRequestBuilder 
{
    public static UpdatePasswordRequest Build(int passwordLength = 10)
    {
        return new Faker<UpdatePasswordRequest>()
            .RuleFor(x => x.CurrentPassword, f => f.Internet.Password())
            .RuleFor(x => x.NewPassword, f => f.Internet.Password(passwordLength));
    }
}