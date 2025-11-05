namespace DiaryAssistance.Core.Exceptions;

public class UnauthorizedException : ApplicationException
{
    public Dictionary<string, string[]>? Errors { get; }

    public UnauthorizedException() { }

    public UnauthorizedException(string message) : base(message) { }

    public UnauthorizedException(string message, Exception inner) : base(message, inner) { }

    public UnauthorizedException(string message, Dictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }
}
