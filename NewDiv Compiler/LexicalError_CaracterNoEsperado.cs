using System;
using System.Collections.Generic;
using System.Text;

namespace NewDiv_Compiler
{
    public class LexicalError_CaracterNoEsperado : LexicalError
    {
        public LexicalError_CaracterNoEsperado(Char c)
            : base("Caracter " + c.ToString() + " no esperado.")
        {
        }
    }
}
