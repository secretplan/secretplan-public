using System.Reflection;
using SokoConsole2;

Type[] types =
[
    typeof(BasicTests),
    typeof(EdgeCaseTests),
    typeof(AnimationTests)
];


var testRunCount = 0;
var testPassCount = 0;
foreach (var type in types)
{
    foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public |
                                           BindingFlags.DeclaredOnly))
    {
        if (method.GetParameters().Length != 0)
        {
            continue;
        }

        var instance = (Activator.CreateInstance(type) as BaseTests)!;
        var failed = false;

        var testName = type.Name + "." + method.Name;

        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        var thread = Task.Run(() =>
        {
            Thread.Sleep(2_000);
            Console.WriteLine("Test took too long, enabling logging!");
            instance.EnableLogging(testName);
        }, token);

        try
        {
            testRunCount++;
            method.Invoke(instance, []);
        }
        catch (TargetInvocationException e)
        {
            Console.WriteLine(testName);
            Console.WriteLine($"FAILED: {e.InnerException?.Message}");
            failed = true;

            try
            {
                Console.WriteLine("Running test again with logging enabled");
                var newInstance = (Activator.CreateInstance(type) as BaseTests)!;
                newInstance.EnableLogging(testName);
                method.Invoke(newInstance, []);
            }
            catch (Exception)
            {
                Console.WriteLine("(logging ends early because of an exception)");
                // ignored
            }
        }

        tokenSource.Cancel();

        if (!failed)
        {
            testPassCount++;
        }
    }
}

Console.WriteLine($"{testPassCount} / {testRunCount} tests passed!");