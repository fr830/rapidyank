using System;
using System.Collections.Generic;
using System.Text;

namespace rapidyank.Rapid
{
    public class RapidException : Exception
    {
        public RapidException(string message, Exception innerException = null)
            : base(message, innerException)
        {

        }
    }
}
