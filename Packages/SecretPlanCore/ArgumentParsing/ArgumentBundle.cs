using System.Reflection;
using SecretPlanCore.Core;

namespace SecretPlanCore.ArgumentParsing;

public static class ArgumentBundle
{
    public static T Generate<T>(List<string> stringArgs) where T : IArgumentBundle
    {
        var instance = Activator.CreateInstance<T>();

        if (instance == null)
        {
            throw new Exception($"Could not create instance of {typeof(T).Name}");
        }

        return Apply(instance, stringArgs);
    }
    
    public static KeyValuePair<string, string>? TryParseCommandLineArgs(string arg)
    {
        arg = arg.Trim();
        if (!arg.StartsWith("--"))
        {
            return null;
        }

        arg = arg.Substring(2);
        if (!arg.Contains("="))
        {
            return new KeyValuePair<string, string>(arg, "true");
        }

        var split = arg.Split("=").ToList();
        var key = split[0];
        var value = arg.Substring(key.Length + 1);
        return new KeyValuePair<string, string>(key, value);
    }

    public static T Apply<T>(T startingValue, List<string> stringArgs)
    {
        var argumentNameToProperty = new Dictionary<string, PropertyInfo>();

        // Collect all argument names
        foreach (var member in Reflection.GetAllMembersInTypeWithAttribute<ArgumentAttribute>(typeof(T)))
        {
            if (member is not PropertyInfo property)
            {
                continue;
            }

            var attribute = property.GetCustomAttribute<ArgumentAttribute>();
            if (attribute == null)
            {
                continue;
            }

            argumentNameToProperty[attribute.Key.ToLower()] = property;
        }

        // Apply argument names
        foreach (var stringArg in stringArgs)
        {
            var keyValuePair = TryParseCommandLineArgs(stringArg);

            if (!keyValuePair.HasValue)
            {
                continue;
            }

            var key = keyValuePair.Value.Key.ToLower();
            var valueAsString = keyValuePair.Value.Value;
            if (!argumentNameToProperty.TryGetValue(key, out var property))
            {
                continue;
            }

            if (Reflection.TryParseTo(property.PropertyType, valueAsString, out var parsedValue))
            {
                property.SetValue(startingValue, parsedValue);
            }
            else
            {
                throw new Exception($"Could not parse `{valueAsString}` as {property.PropertyType.Name}");
            }
        }

        return startingValue;
    }
}