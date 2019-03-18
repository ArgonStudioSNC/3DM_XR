using System;

public class NetworkException : Exception
{
    public NetworkException()
    {
    }

    public NetworkException(string message)
        : base(message)
    {
    }

    public NetworkException(string message, Exception inner)
        : base(message, inner)
    {
    }
}