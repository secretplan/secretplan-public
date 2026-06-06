using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SecretPlanCore.Core;

public static class Reflection
{
    /// <summary>
    ///     Gets static fields from type T that derive from TInterface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TInterface"></typeparam>
    /// <returns></returns>
    [Pure]
    public static Dictionary<string, TInterface> GetStaticFieldsThatDeriveFromType<T, TInterface>()
    {
        return GetStaticFieldsThatDeriveFromType<TInterface>(typeof(T));
    }

    /// <summary>
    ///     Gets static fields from type T that derive from TInterface
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <returns></returns>
    [Pure]
    public static Dictionary<string, TInterface> GetStaticFieldsThatDeriveFromType<TInterface>(Type t)
    {
        return t
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(fieldInfo => fieldInfo.FieldType.GetInterfaces().Contains(typeof(TInterface)) ||
                                fieldInfo.FieldType == typeof(TInterface))
            .ToDictionary(
                fieldInfo => fieldInfo.Name,
                fieldInfo => (TInterface)fieldInfo.GetValue(null)!
            );
    }

    /// <summary>
    ///     Gets static fields from type T that derive from TInterface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TInterface"></typeparam>
    /// <returns></returns>
    [Pure]
    public static IEnumerable<FieldInfo> GetStaticFieldInfosThatDeriveFromType<T, TInterface>()
    {
        return typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(fieldInfo => fieldInfo.FieldType.GetInterfaces().Contains(typeof(TInterface)));
    }

    [Pure]
    public static List<Type> GetAllTypesThatDeriveFrom<T>()
    {
        var inputType = typeof(T);

        // Get all loaded assemblies
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Find all types that implement ISpecificInterface
        var implementingTypes = new List<Type>();

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(type => inputType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            implementingTypes.AddRange(types);
        }

        return implementingTypes;
    }

    [Pure]
    public static IEnumerable<Tuple<MemberInfo, Type>>
        GetAllMembersInAssemblyWithAttribute<TAttribute>(Assembly assembly) where TAttribute : Attribute
    {
        var types = assembly.GetTypes();
        var attributeType = typeof(TAttribute);
        foreach (var type in types)
        {
            foreach (var member in type.GetMembers().Where(method => Attribute.IsDefined(method, attributeType)))
            {
                yield return new Tuple<MemberInfo, Type>(member, type);
            }
        }
    }

    [Pure]
    public static IEnumerable<MemberInfo> GetAllMembersInTypeWithAttribute<TAttribute>(Type type)
        where TAttribute : Attribute
    {
        var attributeType = typeof(TAttribute);
        foreach (var member in type
                     .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                 BindingFlags.Static).Where(member => Attribute.IsDefined(member, attributeType)))
        {
            yield return member;
        }
    }
    
    [Pure]
    public static IEnumerable<MemberInfo> GetAllMembersInTypeWithAttributeFromInstance<TAttribute>(object instance)
        where TAttribute : Attribute
    {
        var attributeType = typeof(TAttribute);
        var members = instance.GetType()
            .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        foreach (var member in members.Where(member => Attribute.IsDefined(member, attributeType)))
        {
            yield return member;
        }
    }

    [Pure]
    public static IEnumerable<Type> GetAllTypesWithAttribute<TAttribute>(Assembly assembly) where TAttribute : Attribute
    {
        var types = assembly.GetTypes();
        var attributeType = typeof(TAttribute);
        foreach (var type in types)
        {
            if (Attribute.IsDefined(type, attributeType))
            {
                yield return type;
            }
        }
    }

    [Pure]
    public static object? GetMemberValue(MemberInfo memberInfo, object instance)
    {
        if (memberInfo is FieldInfo fieldInfo)
        {
            return fieldInfo.GetValue(instance);
        }

        if (memberInfo is PropertyInfo propertyInfo)
        {
            return propertyInfo.GetValue(instance);
        }

        if (memberInfo is MethodInfo methodInfo)
        {
            return methodInfo.Invoke(instance, []);
        }

        throw new Exception($"Could not invoke {memberInfo} on {instance}");
    }

    [Pure]
    public static bool TryParseTo<T>(string input, out T value)
    {
        value = default!;

        if (typeof(T) == typeof(string))
        {
            value = (T)(object)input;
            return true;
        }

        var m = typeof(T).GetMethod(
            "TryParse",
            new[] { typeof(string), typeof(T).MakeByRefType() }
        );

        if (m is null)
        {
            return false;
        }

        var parameters = new object?[] { input, null };
        var success = (bool)m.Invoke(null, parameters)!;

        if (success)
        {
            value = (T)parameters[1]!;
        }

        return success;
    }

    [Pure]
    public static bool TryParseTo(Type type, string input, out object value)
    {
        value = default!;

        if (type == typeof(string))
        {
            value = input;
            return true;
        }

        if (type.IsEnum)
        {
            if (Enum.TryParse(type, input, true, out var enumResult))
            {
                value = enumResult;
                return true;
            }

            return false;
        }


        var m = type.GetMethod(
            "TryParse",
            new[] { typeof(string), type.MakeByRefType() }
        );

        if (m is null)
        {
            return false;
        }

        var parameters = new object?[] { input, null };
        var success = (bool)m.Invoke(null, parameters)!;

        if (success)
        {
            value = parameters[1]!;
        }

        return success;
    }

    public static Type? GetPropertyOrFieldType(MemberInfo member)
    {
        if (member.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
        {
            // ignore compiler generated fields (usually backing fields)
            return null;
        }

        if (member is PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType;
        }

        if (member is FieldInfo fieldInfo)
        {
            return fieldInfo.FieldType;
        }

        return null;
    }

    public static bool IsArrayOrList(Type type)
    {
        if (type.IsArray)
        {
            return true;
        }

        if (!type.IsGenericType)
        {
            return false;
        }

        var genericType = type.GetGenericTypeDefinition();

        return genericType == typeof(IList<>);
    }

    public static object? GetPropertyOrFieldValue<T>(MemberInfo member, T instance)
    {
        if (member is PropertyInfo propertyInfo)
        {
            return propertyInfo.GetValue(instance);
        }

        if (member is FieldInfo fieldInfo)
        {
            return fieldInfo.GetValue(instance);
        }

        return null;
    }

    /// <summary>
    ///     Returns true if `type` has the underlying type `underlyingType`.
    ///     This means `type` is one of the following: 1) exactly `underlyingType` 2) array of `underlyingType` 3) List of
    ///     `underlyingType`
    /// </summary>
    public static bool HasUnderlyingType(Type type, Type underlyingType)
    {
        if (underlyingType.IsAssignableFrom(type))
        {
            return true;
        }

        if (type.IsArray)
        {
            var elementType = type.GetElementType();
            return elementType != null &&
                   underlyingType.IsAssignableFrom(elementType);
        }

        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();

            if (genericType == typeof(List<>))
            {
                var elementType = type.GetGenericArguments()[0];
                return underlyingType.IsAssignableFrom(elementType);
            }
        }

        return false;
    }

    public static void SetMemberValue(object target, string memberName, object? value)
    {
        var type = target.GetType();

        // Try property first
        var prop = type.GetProperty(
            memberName,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );

        if (prop != null)
        {
            var setter = FindSetterForProperty(target.GetType(), memberName);

            if (setter == null)
            {
                throw new InvalidOperationException($"Property '{memberName}' has no setter.");
            }

            var converted = value == null
                ? null
                : Convert.ChangeType(value, prop.PropertyType);

            setter.Invoke(target, [converted]);
            return;
        }

        // Try field
        var field = type.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (field != null)
        {
            field.SetValue(target, value);
            return;
        }

        throw new MissingMemberException(type.FullName, memberName);
    }

    private static MethodInfo? FindSetterForProperty(Type inputType, string name)
    {
        var currentType = inputType;
        while (currentType != null)
        {
            var prop = currentType.GetProperty(name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var setter = prop?.GetSetMethod(true);

            if (setter != null)
            {
                return setter;
            }

            currentType = currentType.BaseType;
        }

        return null;
    }

    /// <summary>
    ///     Returns true if <paramref name="type" /> is or derives from
    ///     the open generic <paramref name="openGeneric" />.
    ///     Example: typeof(List&lt;int&gt;).IsGenericOf(typeof(List&lt;&gt;)) == true
    /// </summary>
    [Pure]
    public static bool IsGenericOf(this Type type, Type openGeneric)
    {
        if (!openGeneric.IsGenericTypeDefinition)
        {
            throw new ArgumentException("openGeneric must be an open generic type (e.g., typeof(List<>))");
        }

        // Walk inheritance chain
        for (var currentType = type; currentType != null; currentType = currentType.BaseType)
        {
            if (currentType.IsGenericType &&
                currentType.GetGenericTypeDefinition() == openGeneric)
            {
                return true;
            }
        }

        // Check interfaces
        return type.GetInterfaces().Any(givenInterface =>
            givenInterface.IsGenericType && givenInterface.GetGenericTypeDefinition() == openGeneric);
    }

    /// <summary>
    ///     Gets the generic arguments if the type matches the open generic.
    ///     Returns null if it does not match.
    /// </summary>
    [Pure]
    public static Type[]? GetGenericArgumentsOf(this Type type, Type openGeneric)
    {
        if (!type.IsGenericOf(openGeneric))
        {
            return null;
        }

        // Check self + base chain first
        for (var current = type; current != null; current = current.BaseType)
        {
            if (current.IsGenericType &&
                current.GetGenericTypeDefinition() == openGeneric)
            {
                return current.GetGenericArguments();
            }
        }

        // Then interfaces
        foreach (var iface in type.GetInterfaces())
        {
            if (iface.IsGenericType &&
                iface.GetGenericTypeDefinition() == openGeneric)
            {
                return iface.GetGenericArguments();
            }
        }

        return null;
    }
}