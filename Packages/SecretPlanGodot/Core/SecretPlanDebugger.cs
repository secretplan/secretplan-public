using System.Reflection;
using SecretPlanCore.ArgumentParsing;
using SecretPlanCore.Core;

namespace SecretPlanGodot.Core;

public abstract class SecretPlanDebugger
{
    public IEnumerable<ConsoleCommand> GenerateConsoleCommands()
    {
        foreach (var member in Reflection.GetAllMembersInTypeWithAttribute<DebugValueAttribute>(GetType()))
        {
            var debugValue = member.GetCustomAttribute<DebugValueAttribute>();
            if (debugValue == null || member is not PropertyInfo property)
            {
                continue;
            }

            var sampleReturnValue = property.GetValue(this);
            if (sampleReturnValue == null)
            {
                continue;
            }

            yield return new ConsoleCommand(debugValue.InvokeWord, args =>
            {
                if (sampleReturnValue is bool)
                {
                    var newValue = !(bool)(property.GetValue(this) ?? false); 
                    if (args.HasIndex(0))
                    {
                        newValue = args.Get(0, "Target Value").ParseAsBool();
                    }
                    property.SetValue(this, newValue);
                    LocalClient.Print($"{property.Name}: {newValue}");
                }
                else if (sampleReturnValue is float)
                {
                    var newValue = args.Get(0, "Target Value").ParseAsFloat();
                    property.SetValue(this, newValue);
                    LocalClient.Print($"{property.Name}: {newValue}");
                }
                else if (sampleReturnValue is int)
                {
                    var newValue = args.Get(0, "Target Value").ParseAsInt();
                    property.SetValue(this, newValue);
                    LocalClient.Print($"{property.Name}: {newValue}");
                }
                else
                {
                    LocalClient.Error($"Unable to interpret type {sampleReturnValue.GetType().Name}");
                }

                CommandRan?.Invoke(debugValue.InvokeWord, args);
            });
        }
    }

    public event Action<string, PositionalArgumentList>? CommandRan;
}