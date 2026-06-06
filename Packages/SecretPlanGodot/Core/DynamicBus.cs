using Godot;

namespace SecretPlanGodot.Core;

public class DynamicBus
{
    private bool _isClosed;
    private static int _busIndex;

    private DynamicBus(string busName)
    {
        BusName = busName;
        // todo: we can subscribe to BusRenamed and BusLayoutChanged to be resilient to name changes
    }

    public string BusName { get; }
    public int BusIndex
    {
        get
        {
            ThrowIfClosed();
            return AudioServer.GetBusIndex(BusName);
        }
    }

    private void ThrowIfClosed()
    {
        if (_isClosed)
        {
            throw new Exception("Attempted to interact with DynamicBus after it was closed");
        }
    }

    public static DynamicBus CreateAndOpen()
    {
        var index = AudioServer.BusCount;
        AudioServer.AddBus();
        
        var name = $"Dynamic Bus {_busIndex++}";
        AudioServer.SetBusName(index, name);
        
        LocalClient.Print($"Created dynamic bus: {name}, at {index}");
        
        return new DynamicBus(name);
    }

    public void Close()
    {
        LocalClient.Print($"Removing dynamic bus {BusName}");
        AudioServer.RemoveBus(BusIndex);
        _isClosed = true;
    }
}