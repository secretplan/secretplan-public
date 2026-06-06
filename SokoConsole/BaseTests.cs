namespace SokoConsole2;

public class BaseTests
{
    private bool _loggingEnabled;

    public BaseTests()
    {
    }

    public void EnableLogging(string prefix)
    {
        if (!_loggingEnabled)
        {
            _loggingEnabled = true;
        }
    }
}