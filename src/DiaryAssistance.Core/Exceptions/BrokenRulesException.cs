namespace DiaryAssistance.Core.Exceptions;

public class BrokenRulesException : ApplicationException
{
    public BrokenRulesException() { }
    public BrokenRulesException(string message) : base(message) { }
    public BrokenRulesException(string message, Exception inner) : base(message, inner) { }
}