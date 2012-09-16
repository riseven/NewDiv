using System;
using System.Collections.Generic;
using System.Text;

namespace NewDiv_Compiler
{
    public class CompileErrorException : Exception
    {
        public CompileErrorException(String msg)
            : base(msg)
        {
        }
    }
}
