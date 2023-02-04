using System.Net.Mail;

namespace Minerva.Utility;

internal static class Util
{
    public static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
        {
            return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        var baseType = givenType.BaseType;
        
        return baseType != null && IsAssignableToGenericType(baseType, genericType);
    }
    
    public static Type[]? GetGenericArgumentsForBaseType(Type givenType, Type genericType)
    {
        if (genericType.IsInterface)
        {
            var interfaceTypes = givenType.GetInterfaces();
            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                {
                    return it.GetGenericArguments();
                }
            }
        }
        else
        {
            var baseType = givenType;
            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == genericType)
                {
                    return baseType.GetGenericArguments();
                }

                baseType = baseType.BaseType;
            }
        }
        return null;
    }
    
    public static Type? GetGenericInterfaceForType(Type givenType, Type genericType)
    {
        if (genericType.IsInterface)
        {
            Type?[] interfaceTypes = givenType.GetInterfaces();
            foreach (var it in interfaceTypes)
            {
                if (it!.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                {
                    return it;
                }
            }
        }
        else
        {
            var baseType = givenType;
            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == genericType)
                {
                    return baseType;
                }

                baseType = baseType.BaseType;
            }
        }
        return null;
    }

    public static MailAddress? ToEmail(this string email)
    {
        try
        {
            return new(email);
        }
        catch (Exception)
        {
            return null;
        }
    }
}