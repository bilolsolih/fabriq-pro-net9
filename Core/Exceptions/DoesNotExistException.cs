using System.Diagnostics.CodeAnalysis;

namespace FabriqPro.Core.Exceptions;

public class DoesNotExistException(string message) : Exception(message)
{
    
    public static void ThrowIfNull([NotNull] dynamic? obj, string modelName)
    {
        if (obj == null)
        {
            throw new DoesNotExistException($"{modelName} does not exist.");
        }
    }
}