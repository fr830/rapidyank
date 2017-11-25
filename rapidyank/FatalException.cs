using System;
using System.Collections.Generic;
using System.Text;

namespace rapidyank
{
    public class FatalException : Exception
    {
        public ExitCode ExitCode { get; set; }

        public FatalException(string message, Exception innerException = null, ExitCode exitCode = 0)
            : base(message, innerException)
        {
            this.ExitCode = exitCode;
        }
    }
}
