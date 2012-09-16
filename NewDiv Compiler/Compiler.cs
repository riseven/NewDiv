using System;
using System.Collections.Generic;
using System.Text;

namespace NewDiv_Compiler
{
    public class Compiler
    {
        public Compiler()
        {
        }

        

        public String Compile(String program)
        {
            // Primero, pasamos la entrada al analizador lexico
            Lexical lex = new Lexical(program);
            
            String result = "";
            LexicalToken token = lex.NextToken();
            while (token.Tipo != TipoToken.EOF)
            {
                result += token.Lexema + "\r\n";
                token = lex.NextToken();
            }

            return result;
        }
    }
}
