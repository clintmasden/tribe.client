using System;

namespace Tribe.Client.Exceptions
{
    public class AccessTokenException : Exception
    {
        public AccessTokenException(string message) : base(message)
        {
        }
    }
}