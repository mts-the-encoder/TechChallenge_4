using System.Runtime.Serialization;

namespace Exceptions.ExceptionBase;

public class InvalidLoginException : TechChallengeException
{
    public InvalidLoginException() : base("Login inválido") { }

    protected InvalidLoginException(SerializationInfo info,StreamingContext context) : base(info,context)
    {
    }
}