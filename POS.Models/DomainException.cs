namespace POS.Models;

public sealed class DomainException(string message) : Exception(message);
