using System;
using System.Collections.Generic;
using System.Text;

namespace NewDiv_Compiler
{
    class Lexical
    {
        private String programa;
        private int readingOffset;
        private int sureOffset;
        private int tokenReadingOffset;

        private enum Estado
        {
            Inicial,
            EnComentario,
            Id,
            PYC,
            OpAsig,
            OpComp,
            Coma,
            CorI,
            CorD,
            LlaveI,
            LlaveD,
            ParI,
            ParD,
            DosPuntos,
            DoblePunto,
            OpAcc,
            OpAsigOp,
            OpComp_Mayor,
            OpComp_Menor,
            OpRot,
            OpRot_Izq,
            OpRot_Der,
            OpLog,
            OpLog_And,
            OpLog_Or,
            OpLog_Xor,
            OpSomb,
            OpSuma,
            OpSuma_Mas,
            OpSuma_Menos,
            OpInc,
            OpMult,
            OpAstx,
            OpMult_Por,
            OpMult_Div,
            OpMult_Mod,
            OpIndir,
            OpNegLog,
            NumEntero,
            Cadena,
        }

        public Lexical(String prog)
        {
            programa = prog;
            readingOffset = 0;
            sureOffset = 0;
            tokenReadingOffset = 0;
        }

        public LexicalToken NextToken()
        {
            Estado estado = Estado.Inicial;
            bool salir = false ;
            tokenReadingOffset = readingOffset;
            sureOffset = readingOffset;

            while ( !salir )
            {
                if (tokenReadingOffset == programa.Length)
                {
                    // Si hemos llegado al final del programa, y estamos en comentario
                    // es un error.
                    if (estado == Estado.EnComentario)
                    {
                        throw new LexicalError_FinDeFicheroNoEsperado();
                    }

                    // Si hemos llegado al final del programa, y no hemos leido aun nada
                    // es el fin del fichero
                    if (estado == Estado.Inicial)
                    {
                        LexicalToken eofToken = new LexicalToken();
                        eofToken.Tipo = TipoToken.EOF;
                        eofToken.Lexema = "";
                        return eofToken;
                    }

                    salir = true;
                }
                else
                {

                    // Leemos un nuevo caracter
                    Char c = programa[tokenReadingOffset];
                    tokenReadingOffset++;

                    // Ejecutamos la transicion en la FSM
                    switch (estado)
                    {
                        case Estado.Inicial:
                            if (Char.IsWhiteSpace(c))
                            {
                                // Ignoramos el caracter
                                readingOffset++;
                                sureOffset = readingOffset;
                                tokenReadingOffset = readingOffset;
                            }
                            else if (Char.IsLetter(c))
                            {
                                // Ahora tenemos un id
                                estado = Estado.Id;
                                // Y ademas es valido
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == ';')
                            {
                                // Hemos leido PYC y no puede venir nada detras
                                estado = Estado.PYC;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '=')
                            {
                                // De momento tenemos = (aunque puede venir ==, =>, =<)
                                estado = Estado.OpAsig;
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == ',')
                            {
                                // Nos quedamos con la coma y no puede venir nada detras
                                estado = Estado.Coma;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '[')
                            {
                                // Hemos leido [ y no puede venir nada detras
                                estado = Estado.CorI;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == ']')
                            {
                                // Hemos leido ] y no puede venir nada detras
                                estado = Estado.CorD;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '{')
                            {
                                // Hemos leido { y no puede venir nada detras
                                estado = Estado.LlaveI;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '}')
                            {
                                // Hemos leido } y no puede venir nada detras
                                estado = Estado.LlaveD;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '(')
                            {
                                // Hemos leido ( y no puede venir nada detras
                                estado = Estado.ParI;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == ')')
                            {
                                // Hemos leido ) y no puede venir nada detras
                                estado = Estado.ParD;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == ':')
                            {
                                // Hemos leido : y no puede venir nada detras
                                estado = Estado.DosPuntos;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '.')
                            {
                                // Hemos leido . pero aun puede ser ..
                                estado = Estado.OpAcc;
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == '<')
                            {
                                // Hemos leido <, puede venir <>, <<, <=
                                estado = Estado.OpComp_Menor;
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == '>')
                            {
                                // Hemos leido >, puede venir >>, >=
                                estado = Estado.OpComp_Mayor;
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == '&')
                            {
                                // Hemos leido &, puede venir && o &=
                                estado = Estado.OpLog_And;
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == '|')
                            {
                                // Hemos leido |, puede venir || o |=
                                estado = Estado.OpLog_Or;
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == '^')
                            {
                                // Hemos leido ^, puede venir ^^ o ^=
                                estado = Estado.OpLog_Xor;
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == '+')
                            {
                                // Hemos leido +, puede venir ++ o +=
                                estado = Estado.OpSuma_Mas;
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == '-')
                            {
                                // Hemos leido -, puede venir -- o -=
                                estado = Estado.OpSuma_Menos;
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == '*')
                            {
                                // Hemos leido *, puede venir *=
                                estado = Estado.OpMult_Por;
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == '/')
                            {
                                // Hemos leido /, puede venir /=
                                estado = Estado.OpMult_Div;
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == '%')
                            {
                                // Hemos leido %, puede venir %=
                                estado = Estado.OpMult_Mod;
                                sureOffset = tokenReadingOffset;
                            }
                            else if (c == '!')
                            {
                                // Hemos leido !, puede venir !=
                                estado = Estado.OpNegLog;
                                sureOffset = tokenReadingOffset;
                            }
                            else
                            {
                                throw new LexicalError_CaracterNoEsperado(c);
                            }
                            break;
                        case Estado.EnComentario:
                            break;
                        case Estado.OpAcc:
                            if (c == '.')
                            {
                                // Hemos leido .. y no puede venir nada detras
                                estado = Estado.DoblePunto;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Al final solo era .
                                salir = true;
                            }
                            break;
                        case Estado.Id:
                            if (Char.IsLetterOrDigit(c))
                            {
                                // Seguimos teniendo un id
                                sureOffset = tokenReadingOffset;
                            }
                            else
                            {
                                // Se acabo el token (id)
                                salir = true;
                            }
                            break;
                        case Estado.OpAsig:
                            if (c == '=')
                            {
                                // Tenemos == y no puede venir nada detras
                                estado = Estado.OpComp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '>')
                            {
                                // Tenemos => y no puede venir nada detras
                                estado = Estado.OpComp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '<')
                            {
                                // Tenemos =< y no puede venir nada detras
                                estado = Estado.OpComp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Nos quedamos con el =
                                salir = true;
                            }
                            break;
                        case Estado.OpComp_Menor:
                            if (c == '>')
                            {
                                // Hemos leido <> y no puede venir nada detras
                                estado = Estado.OpComp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '=')
                            {
                                // Hemos leido <= y no puede venir nada detras
                                estado = Estado.OpComp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '<')
                            {
                                // Hemos leido << y puede venir <<=
                                estado = Estado.OpRot_Izq;
                                sureOffset = tokenReadingOffset;
                            }
                            else
                            {
                                // Al final solo era <
                                estado = Estado.OpComp;
                                salir = true;
                            }
                            break;
                        case Estado.OpComp_Mayor:
                            if (c == '>')
                            {
                                // Hemos leido >> y puede venir >>=
                                estado = Estado.OpRot_Der;
                                sureOffset = tokenReadingOffset;
                            }
                            else if ( c == '=' )
                            {
                                // Hemos leido >= y no puede venir nada mas
                                estado = Estado.OpComp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else 
                            {
                                // Solo era >
                                estado = Estado.OpComp;
                                salir = true;
                            }
                            break;
                        case Estado.OpRot_Der:
                            if (c == '=')
                            {
                                // Hemos leido >>= y no puede venir nada detras
                                estado = Estado.OpAsigOp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Solo era >>
                                estado = Estado.OpRot;
                                salir = true;
                            }
                            break;
                        case Estado.OpRot_Izq:
                            if (c == '=')
                            {
                                // Hemos leido <<= y no puede venir nada detras
                                estado = Estado.OpAsigOp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Solo era <<
                                estado = Estado.OpRot;
                                salir = true;
                            }
                            break;
                        case Estado.OpLog_And:
                            if (c == '&')
                            {
                                // Hemos leido && y no puede venir nada detras
                                estado = Estado.OpLog;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '=')
                            {
                                // Hemos leido &= y no puede venir nada detras
                                estado = Estado.OpAsigOp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Solo era &
                                estado = Estado.OpIndir;
                                salir = true;
                            }
                            break;
                        case Estado.OpLog_Or:
                            if (c == '|')
                            {
                                // Hemos leido || y no puede venir nada detras
                                estado = Estado.OpLog;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '=')
                            {
                                // Hemos leido |= y no puede venir nada detras
                                estado = Estado.OpAsigOp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Solo era |
                                estado = Estado.OpLog;
                                salir = true;
                            }
                            break;
                        case Estado.OpLog_Xor:
                            if (c == '^')
                            {
                                // Hemos leido ^^ y no puede venir nada detras
                                estado = Estado.OpLog;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '=')
                            {
                                // Hemos leido ^= y no puede venir nada detras
                                estado = Estado.OpAsigOp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Solo era ^
                                estado = Estado.OpSomb;
                                salir = true;
                            }
                            break;
                        case Estado.OpSuma_Mas:
                            if (c == '+')
                            {
                                // Hemos leido ++, no puede venir nada mas
                                estado = Estado.OpInc;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '=')
                            {
                                // Hemos leido +=, no puede venir nada mas
                                estado = Estado.OpAsigOp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Solo era +
                                estado = Estado.OpSuma;
                                salir = true;
                            }
                            break;
                        case Estado.OpSuma_Menos:
                            if (c == '-')
                            {
                                // Hemos leido --, no puede venir nada mas
                                estado = Estado.OpInc;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else if (c == '=')
                            {
                                // Hemos leido -=, no puede venir nada mas
                                estado = Estado.OpAsigOp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Solo era -
                                estado = Estado.OpSuma;
                                salir = true;
                            }
                            break;
                        case Estado.OpMult_Por:
                            if (c == '=')
                            {
                                // Hemos leido *=, no puede venir nada mas
                                estado = Estado.OpAsigOp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Solo era *
                                estado = Estado.OpAstx;
                                salir = true;
                            }
                            break;
                        case Estado.OpMult_Div:
                            if (c == '/')
                            {
                                // Hemos leido /=, no puede venir nada mas
                                estado = Estado.OpAsigOp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Solo era /
                                estado = Estado.OpMult;
                                salir = true;
                            }
                            break;
                        case Estado.OpMult_Mod:
                            if (c == '%')
                            {
                                // Hemos leido %=, no puede venir nada mas
                                estado = Estado.OpAsigOp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Solo era %
                                estado = Estado.OpMult;
                                salir = true;
                            }
                            break;
                        case Estado.OpNegLog:
                            if (c == '=')
                            {
                                // Hemos leido !=, no puede venir nada mas
                                estado = Estado.OpComp;
                                sureOffset = tokenReadingOffset;
                                salir = true;
                            }
                            else
                            {
                                // Solo era !
                                salir = true;
                            }
                            break;
                        default:
                            throw new Exception("Error fatal en el analizador léxico");
                    }
                }
            }

            // Si no tenemos token, es un error lexico asociado al primer elemento
            // leido
            if (sureOffset == readingOffset)
            {
                throw new LexicalError_CaracterNoEsperado(programa[sureOffset]);
            }

            // Si tenemos token, lo devolvemos
            LexicalToken token = new LexicalToken();
            token.Lexema = programa.Substring(readingOffset, sureOffset - readingOffset);

            switch (estado)
            {
                case Estado.OpAsig:
                    token.Tipo = TipoToken.OpAsig;
                    break;
                case Estado.OpComp:
                    token.Tipo = TipoToken.OpComp;
                    break;
                case Estado.Coma:
                    token.Tipo = TipoToken.Coma;
                    break;
                case Estado.CorI:
                    token.Tipo = TipoToken.CorI;
                    break;
                case Estado.CorD:
                    token.Tipo = TipoToken.CorD;
                    break;
                case Estado.LlaveI:
                    token.Tipo = TipoToken.LlaveI;
                    break;
                case Estado.LlaveD:
                    token.Tipo = TipoToken.LlaveD;
                    break;
                case Estado.ParI:
                    token.Tipo = TipoToken.ParI;
                    break;
                case Estado.ParD:
                    token.Tipo = TipoToken.ParD;
                    break;
                case Estado.DosPuntos:
                    token.Tipo = TipoToken.DosPuntos;
                    break;
                case Estado.DoblePunto:
                    token.Tipo = TipoToken.DoblePunto;
                    break;
                case Estado.OpAsigOp:
                    token.Tipo = TipoToken.OpAsigOp;
                    break;
                case Estado.OpRot:
                    token.Tipo = TipoToken.OpRot;
                    break;
                case Estado.OpSomb:
                    token.Tipo = TipoToken.OpSomb;
                    break;
                case Estado.OpSuma:
                    token.Tipo = TipoToken.OpSuma;
                    break;
                case Estado.OpInc:
                    token.Tipo = TipoToken.OpInc;
                    break;
                case Estado.OpMult:
                    token.Tipo = TipoToken.OpMult;
                    break;
                case Estado.OpAstx:
                    token.Tipo = TipoToken.OpAstx;
                    break;
                case Estado.OpIndir:
                    token.Tipo = TipoToken.OpIndir;
                    break;
                case Estado.OpNegLog:
                    token.Tipo = TipoToken.OpNegLog;
                    break;
                case Estado.Id:
                    // Comprobamos si es una palabra reservada
                    String lower = token.Lexema.ToLower();
                    if (lower == "program")
                    {
                        token.Tipo = TipoToken.Program;
                    }
                    else if (lower == "const")
                    {
                        token.Tipo = TipoToken.Const;
                    }
                    else if (lower == "global")
                    {
                        token.Tipo = TipoToken.Global;
                    }
                    else if (lower == "private")
                    {
                        token.Tipo = TipoToken.Private;
                    }
                    else if (lower == "struct")
                    {
                        token.Tipo = TipoToken.Struct;
                    }
                    else if (lower == "end")
                    {
                        token.Tipo = TipoToken.End;
                    }
                    else if (lower == "dup")
                    {
                        token.Tipo = TipoToken.Dup;
                    }
                    else if (lower == "process")
                    {
                        token.Tipo = TipoToken.Process;
                    }
                    else if (lower == "function")
                    {
                        token.Tipo = TipoToken.Function;
                    }
                    else if (lower == "begin")
                    {
                        token.Tipo = TipoToken.Begin;
                    }
                    else if (lower == "if")
                    {
                        token.Tipo = TipoToken.If;
                    }
                    else if (lower == "switch")
                    {
                        token.Tipo = TipoToken.Switch;
                    }
                    else if (lower == "while")
                    {
                        token.Tipo = TipoToken.While;
                    }
                    else if (lower == "repeat")
                    {
                        token.Tipo = TipoToken.Repeat;
                    }
                    else if (lower == "until")
                    {
                        token.Tipo = TipoToken.Until;
                    }
                    else if (lower == "loop")
                    {
                        token.Tipo = TipoToken.Loop;
                    }
                    else if (lower == "from")
                    {
                        token.Tipo = TipoToken.From;
                    }
                    else if (lower == "for")
                    {
                        token.Tipo = TipoToken.For;
                    }
                    else if (lower == "break")
                    {
                        token.Tipo = TipoToken.Break;
                    }
                    else if (lower == "continue")
                    {
                        token.Tipo = TipoToken.Continue;
                    }
                    else if (lower == "return")
                    {
                        token.Tipo = TipoToken.Return;
                    }
                    else if (lower == "frame")
                    {
                        token.Tipo = TipoToken.Frame;
                    }
                    else if (lower == "clone")
                    {
                        token.Tipo = TipoToken.Clone;
                    }
                    else if (lower == "debug")
                    {
                        token.Tipo = TipoToken.Debug;
                    }
                    else if (lower == "push_state")
                    {
                        token.Tipo = TipoToken.PushState;
                    }
                    else if (lower == "pop_state")
                    {
                        token.Tipo = TipoToken.PopState;
                    }
                    else if (lower == "case")
                    {
                        token.Tipo = TipoToken.Case;
                    }
                    else if (lower == "default")
                    {
                        token.Tipo = TipoToken.Default;
                    }
                    else if (lower == "step")
                    {
                        token.Tipo = TipoToken.Step;
                    }
                    else if (lower == "char")
                    {
                        token.Tipo = TipoToken.Char;
                    }
                    else if (lower == "byte")
                    {
                        token.Tipo = TipoToken.Byte;
                    }
                    else if (lower == "short")
                    {
                        token.Tipo = TipoToken.Short;
                    }
                    else if (lower == "word")
                    {
                        token.Tipo = TipoToken.Word;
                    }
                    else if (lower == "int")
                    {
                        token.Tipo = TipoToken.Int;
                    }
                    else if (lower == "dword")
                    {
                        token.Tipo = TipoToken.DWord;
                    }
                    else if (lower == "long")
                    {
                        token.Tipo = TipoToken.Long;
                    }
                    else if (lower == "qword")
                    {
                        token.Tipo = TipoToken.QWord;
                    }
                    else if (lower == "float")
                    {
                        token.Tipo = TipoToken.Float;
                    }
                    else if (lower == "double")
                    {
                        token.Tipo = TipoToken.Double;
                    }
                    else if (lower == "string")
                    {
                        token.Tipo = TipoToken.String;
                    }
                    else if (lower == "bool")
                    {
                        token.Tipo = TipoToken.Bool;
                    }
                    else if (lower == "decimal")
                    {
                        token.Tipo = TipoToken.Decimal;
                    }
                    else if (lower == "and")
                    {
                        token.Tipo = TipoToken.OpLog;
                    }
                    else if (lower == "or")
                    {
                        token.Tipo = TipoToken.OpLog;
                    }
                    else if (lower == "xor")
                    {
                        token.Tipo = TipoToken.OpLog;
                    }
                    else if (lower == "mod")
                    {
                        token.Tipo = TipoToken.OpMult;
                    }
                    else if (lower == "not")
                    {
                        token.Tipo = TipoToken.OpNegLog;
                    }
                    else if (lower == "true" || lower == "false")
                    {
                        token.Tipo = TipoToken.Booleano;
                    }
                    else
                    {
                        token.Tipo = TipoToken.Id;
                    }
                    break;
                default:
                    throw new Exception("Error fatal en el analizador léxico");
            }

            // Avanzamos la posicion de lectura
            readingOffset = sureOffset;
            return token;
        }
    }
}
