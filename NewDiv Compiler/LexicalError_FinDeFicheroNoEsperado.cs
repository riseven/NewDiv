using System;
using System.Collections.Generic;
using System.Text;

namespace NewDiv_Compiler
{
    public class LexicalError_FinDeFicheroNoEsperado : LexicalError
    {
        public LexicalError_FinDeFicheroNoEsperado()
            : base("Se alcanzó el final del fichero en comentario")
        {
        }
    }
}
