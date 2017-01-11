using System;

namespace Kantilever.Magazijnbeheer.Shared
{

    [Serializable]
    public class ArtikelOnbekendException : System.Exception
    {
        public ArtikelOnbekendException() { }
        public ArtikelOnbekendException(string message) : base(message) { }
        public ArtikelOnbekendException(string message, Exception inner) : base(message, inner) { }
    }
}