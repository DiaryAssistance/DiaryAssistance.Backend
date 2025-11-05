namespace DiaryAssistance.Core.Exceptions;

public class ConflictException : ApplicationException
{
    public Dictionary<string, string[]>? Errors { get; }

    public ConflictException() { }

    public ConflictException(string message) : base(message) { }

    public ConflictException(string message, Exception inner) : base(message, inner) { }

    public ConflictException(string message, Dictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }
}
