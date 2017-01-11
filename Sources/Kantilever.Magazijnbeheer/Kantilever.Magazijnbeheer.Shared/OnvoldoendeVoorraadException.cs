using System;

namespace Kantilever.Magazijnbeheer.Shared
{
    [Serializable]
    public class OnvoldoendeVoorraadException : System.Exception
    {
        public OnvoldoendeVoorraadException() { }
        public OnvoldoendeVoorraadException(string message) : base(message) { }
        public OnvoldoendeVoorraadException(string message, Exception inner) : base(message, inner) { }
    }
}