namespace Paluwagan.SharedKernel.Exceptions;

/// <summary>
/// Base exception for all domain-specific exceptions.
/// </summary>
public class BaseException(string? message = null, Exception? innerException = null)
    : Exception(message, innerException);

/// <summary>
/// Exception thrown when a specific domain business rule is violated.
/// </summary>
public class BusinessRuleBrokenException(string message, Exception? innerException = null)
    : BaseException(message, innerException);

public class NotFoundException(string message, Exception? innerException = null)
      : BaseException(message, innerException);
public class UnauthorizedException(string message, Exception? innerException = null)
    : BaseException(message, innerException);
