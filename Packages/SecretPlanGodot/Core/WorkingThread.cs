using System.Collections.Concurrent;

namespace SecretPlanGodot.Core;

public class WorkingThread
{
    private readonly ConcurrentQueue<Action> _queue = new();
    private bool _threadIsWorking;
    private Task? _workingThread;
    private readonly string _name;

    public WorkingThread(string name)
    {
        _name = name;
    }
    
    public void Start()
    {
        _threadIsWorking = true;
        _workingThread = Task.Run(WorkingThreadLoop);
        LocalClient.Print($"{_name} Working thread started");
    }

    public void AddWork(Action work)
    {
        if (!_threadIsWorking)
        {
            LocalClient.Error($"{_name} Gave working thread work while it wasn't running"); // not technically an error, should probably be a warning?
        }
        
        _queue.Enqueue(work);
    }

    public void WorkingThreadLoop()
    {
        while (_threadIsWorking)
        {
            if (_queue.TryDequeue(out var result))
            {
                result();
            }
        }

        LocalClient.Print($"{_name} Worker thread terminated");
    }

    public void StopAndWait()
    {
        _threadIsWorking = false;
        _workingThread?.Wait();
    }
}