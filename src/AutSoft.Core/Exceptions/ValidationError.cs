namespace AutSoft.Common.Exceptions;

public class ValidationError
{
    public ValidationError(string key, string error)
    {
        Key = key;
        Error = error;
    }

    public string Key { get; }

    public string Error { get; }
}
