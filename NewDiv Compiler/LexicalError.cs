using System;
using System.Collections.Generic;
using System.Text;

namespace NewDiv_Compiler
{
    public class LexicalError : CompileErrorException
    {
        public LexicalError(String msg) : base(msg)
        {
        }
    }
}
