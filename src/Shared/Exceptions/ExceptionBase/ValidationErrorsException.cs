using System.Runtime.Serialization;

namespace Exceptions.ExceptionBase;

[Serializable]
public class ValidationErrorsException : TechChallengeException
{
    public List<string> ErrorMessages { get; set; }

    public ValidationErrorsException(List<string> errorMessages) : base(string.Empty)
    {
        ErrorMessages = errorMessages;
    }

    protected ValidationErrorsException(SerializationInfo info,StreamingContext context) : base(info, context)
    {
    }
}