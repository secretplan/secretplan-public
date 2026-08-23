using System.Reflection;
using SecretPlanCore.ArgumentParsing;
using SecretPlanCore.Core;

namespace SecretPlanGodot.Core;

public abstract class SecretPlanDebugger
{
    public IEnumerable<ConsoleCommand> GenerateConsoleCommands(bool skipHidden)
    {
        foreach (var member in Reflection.GetAllMembersInTypeWithAttribute<DebugValueAttribute>(GetType()))
        {
            var debugValue = member.GetCustomAttribute<DebugValueAttribute>();
            if (debugValue == null)
            {
                continue;
            }
            
            if (skipHidden && debugValue.IsHidden)
            {
                continue;
            }
            
            if (member is MethodInfo method)
            {
                yield return CreateConsoleCommandFromMethod(debugValue, method);
            }

            if (member is PropertyInfo property)
            {
                var sampleReturnValue = property.GetValue(this);
                if (sampleReturnValue == null)
                {
                    continue;
                }

                yield return CreateConsoleCommandFromProperty(debugValue, sampleReturnValue.GetType(), property);
            }
        }
    }

    private ConsoleCommand CreateConsoleCommandFromMethod(DebugValueAttribute debugValue, MethodInfo method)
    {
        return new ConsoleCommand(debugValue.InvokeWord, args =>
        {
            var argIndex = 0;
            var arguments = new object[method.GetParameters().Length];
            foreach (var methodParameter in method.GetParameters())
            {
                arguments[argIndex] = args.Get(argIndex, methodParameter.Name ?? "param").ParseAsType(methodParameter.ParameterType);
                argIndex++;
            }

            var result = method.Invoke(this, arguments);

            if (result != null)
            {
                LocalClient.Print(result);
            }

            CommandRan?.Invoke(debugValue.InvokeWord, args);
        });
    }

    private ConsoleCommand CreateConsoleCommandFromProperty(DebugValueAttribute debugValue, Type returnType, PropertyInfo property)
    {
        return new ConsoleCommand(debugValue.InvokeWord, args =>
        {
            if (returnType == typeof(bool))
            {
                var newValue = !(bool)(property.GetValue(this) ?? false);
                if (args.HasIndex(0))
                {
                    newValue = args.Get(0, "Target Value").ParseAsBool();
                }

                property.SetValue(this, newValue);
                LocalClient.Print($"{property.Name}: {newValue}");
            }
            else if (returnType == typeof(float))
            {
                var newValue = args.Get(0, "Target Value").ParseAsFloat();
                property.SetValue(this, newValue);
                LocalClient.Print($"{property.Name}: {newValue}");
            }
            else if (returnType == typeof(int))
            {
                var newValue = args.Get(0, "Target Value").ParseAsInt();
                property.SetValue(this, newValue);
                LocalClient.Print($"{property.Name}: {newValue}");
            }
            else
            {
                LocalClient.Error($"Unable to interpret type {returnType.Name}");
            }

            CommandRan?.Invoke(debugValue.InvokeWord, args);
        });
    }

    public event Action<string, PositionalArgumentList>? CommandRan;
}