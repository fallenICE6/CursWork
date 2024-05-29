using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11
{
    public enum TokenType
    {
        DIM, INTEGER, AS, INDENTIFIER,  EQUAL, LITERAL, SELECT, CASE, PLUS, MULTIPLICATION, LPAR, RPAR, TO, ELSE, END, NEWSTRING, COMMA, NETERMINAL, BOOLEAN, BYTE, MINUS, DIVIDE
    }

    public class Token
    {
        public TokenType Type;
        public string Value;
        public Token(TokenType type) { Type = type; }
        public string ToString()
        {
            return string.Format($"{Type}, {Value}");
        }
    }


}
