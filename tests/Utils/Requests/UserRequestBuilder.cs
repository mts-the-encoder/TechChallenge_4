﻿using Bogus;
using Communication.Requests;

namespace Utils.Requests;

public class UserRequestBuilder
{
    public static UserRequest Build(int passwordLength = 10)
    {
        return new Faker<UserRequest>()
			.RuleFor(x => x.Name, f => f.Person.FullName)
			.RuleFor(x => x.Email, f => f.Person.Email)
			.RuleFor(x => x.YearBorn, f => f.Person.DateOfBirth.AddYears(18).ToLongDateString())
			.RuleFor(x => x.Password, f => f.Internet.Password(passwordLength));
    }
}