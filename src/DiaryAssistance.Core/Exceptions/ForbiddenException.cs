namespace DiaryAssistance.Core.Exceptions;

public class ForbiddenException : ApplicationException
{
    public Dictionary<string, string[]>? Errors { get; }

    public ForbiddenException() { }

    public ForbiddenException(string message) : base(message) { }

    public ForbiddenException(string message, Exception inner) : base(message, inner) { }

    public ForbiddenException(string message, Dictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }
}
