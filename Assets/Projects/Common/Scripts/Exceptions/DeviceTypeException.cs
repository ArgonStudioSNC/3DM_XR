using System;

public class DeviceTypeException : Exception
{
    public DeviceTypeException()
    {
    }

    public DeviceTypeException(string message)
        : base(message)
    {
    }

    public DeviceTypeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}