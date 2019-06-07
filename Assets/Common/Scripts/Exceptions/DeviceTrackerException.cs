using System;

public class DeviceTrackerException : Exception
{
    public DeviceTrackerException()
    {
    }

    public DeviceTrackerException(string message)
        : base(message)
    {
    }

    public DeviceTrackerException(string message, Exception inner)
        : base(message, inner)
    {
    }
}