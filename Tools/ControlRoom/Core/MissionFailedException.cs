using System;

namespace ControlRoom.Core;

public class MissionFailedException : Exception
{
    public MissionFailedException(string message, Exception? inner = null) : base(message, inner)
    {
    }
}
