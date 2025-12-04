namespace Iara.Domain.Exceptions;

/// <summary>
/// Base exception for domain-specific errors
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }

    protected DomainException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when entity is not found
/// </summary>
public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when business rule is violated
/// </summary>
public class BusinessRuleViolationException : DomainException
{
    public BusinessRuleViolationException(string message) : base(message)
    {
    }
}

/// <summary>
/// Exception thrown when unauthorized action is attempted
/// </summary>
public class UnauthorizedAccessException : DomainException
{
    public UnauthorizedAccessException(string message) 
        : base(message)
    {
    }
}

/// <summary>
/// Exception thrown when validation fails
/// </summary>
public class ValidationException : DomainException
{
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException(Dictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }
}
