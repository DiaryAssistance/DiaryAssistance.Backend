namespace DiaryAssistance.Core.Exceptions;

public class InternalServerException : ApplicationException
{
    public Dictionary<string, string[]>? Errors { get; }

    public InternalServerException() { }

    public InternalServerException(string message) : base(message) { }

    public InternalServerException(string message, Exception inner) : base(message, inner) { }

    public InternalServerException(string message, Dictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }
}
