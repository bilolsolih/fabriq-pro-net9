namespace FabriqPro.Core.Exceptions;

public sealed class AlreadyExistsException(string message) : Exception(message)
{
  public static void ThrowIf(bool condition, string message)
  {
    if (condition)
      throw new AlreadyExistsException(message);
  }
}