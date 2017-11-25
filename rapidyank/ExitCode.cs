using System;
using System.Collections.Generic;
using System.Text;

namespace rapidyank
{
    public enum ExitCode
    {
        Ok = 0,
        BadArguments = 1,
        BadConfig = 2,
        BadData = 3,
        BadProgram = 4,
        CommunicationError = 5,
        RapidError = 19,        
        UnknownError = 99,
    }
}
