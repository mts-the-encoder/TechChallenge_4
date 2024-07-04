using System.Runtime.Serialization;

namespace Exceptions.ExceptionBase;

[Serializable]
public class TechChallengeException : SystemException
{
    public TechChallengeException(string message) : base(message)
    {
    }

    protected TechChallengeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}