using System;
using System.Collections.Generic;
using System.Text;

namespace NewDiv_Compiler
{
    public class LexicalToken
    {
        private String lexema;
        private TipoToken tipoTok;

        public String Lexema
        {
            get
            {
                return lexema;
            }
            set
            {
                lexema = value;
            }
        }
        public TipoToken Tipo
        {
            get
            {
                return tipoTok;
            }
            set
            {
                tipoTok = value;
            }
        }

        public LexicalToken()
        {
            lexema = "";
        }
    }
}
