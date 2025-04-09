using System.Diagnostics.CodeAnalysis;

namespace FabriqPro.Core.Exceptions;

public sealed class DoesNotExistException(string message) : Exception(message)
{
  public static void ThrowIfNull([NotNull] dynamic? obj, string message)
  {
    if (obj == null)
      throw new DoesNotExistException(message);
  }

  public static void ThrowIfNot(bool condition, string message)
  {
    if (!condition)
      throw new DoesNotExistException(message);
  }
}