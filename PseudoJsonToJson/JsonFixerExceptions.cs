using System;

namespace JsonFixer
{
    public class FailedToFixJsonException : Exception
    {
        public FailedToFixJsonException(string message)
            : base(message)
        {
        }
    }
}