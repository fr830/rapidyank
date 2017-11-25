using System;
using System.Collections.Generic;
using System.Text;

namespace rapidyank
{
    public class CommunicationException : Exception
    {
        public CommunicationException(string message, Exception innerException = null)
            : base(message, innerException)
        {

        }
    }
}
