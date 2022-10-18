using Recipes.Domain.Primitives;

namespace Recipes.Domain.Shared;

public class Error : ValueObject
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");
    public Error(string code, string message, string? originalProperyName = null)
    {
        Code = code;
        Message = message;
        OriginalPropertyName = originalProperyName;
        StackTrace = Environment.StackTrace;
    }
    protected string StackTrace { get;}
    public string Code { get; }
    public string Message { get; }
    public string? OriginalPropertyName { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Code;
        yield return Message;
        yield return OriginalPropertyName;
    }

    public static implicit operator string(Error error) => error.Code;
}
