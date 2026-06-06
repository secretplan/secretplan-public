using System.Reflection;
using SecretPlanCore.Core;
using SecretPlanCore.Extensions;
using SecretPlanGodot.Core;

namespace SecretPlanGodot.Testing;

public static class TestHelpers
{
    private static IEnumerable<object> InstantiateAllTestObjects(Assembly assembly)
    {
        foreach (var type in Reflection.GetAllTypesWithAttribute<RuntimeUnitTestClassAttribute>(assembly))
        {
            var result = Activator.CreateInstance(type);

            if (result != null)
            {
                yield return result;
            }
        }
    }

    public static void RunAllTestsInAssembly(Assembly assembly)
    {
        foreach (var instance in InstantiateAllTestObjects(assembly))
        {
            foreach (var member in Reflection.GetAllMembersInTypeWithAttributeFromInstance<RuntimeTestAttribute>(
                         instance))
            {
                if (member is MethodInfo method)
                {
                    LocalClient.Print($"Running: {instance}.{member.Name}");
                    try
                    {
                        method.Invoke(instance, []);
                    }
                    catch (Exception e)
                    {
                        var assertionFailedException = e.SearchInnerUntilFound<AssertionFailedException>();
                        if (assertionFailedException != null)
                        {
                            LocalClient.Error($"{member.Name} FAILED: {assertionFailedException.Message}");
                        }

                        continue;
                    }

                    LocalClient.Print("Passed!");
                }
            }
        }
    }
}