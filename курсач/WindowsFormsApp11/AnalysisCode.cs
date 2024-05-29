using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11
{
    class AnalysisCode
    {
        public List<Token> tokens = new List<Token>();
        static Dictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>()
        {
        { "Dim", TokenType.DIM },
        { "as", TokenType.AS },
        { "integer", TokenType.INTEGER },
        { "select", TokenType.SELECT },
        { "case", TokenType.CASE },
        { "to", TokenType.TO },
        { "end", TokenType.END },
        { "else", TokenType.ELSE },
        { "boolean", TokenType.BOOLEAN },
        { "byte", TokenType.BYTE }
        };
        public static bool IsSpecialWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }
            return (Keywords.ContainsKey(word));
        }
        static Dictionary<string, TokenType> SpecialSymbols =
        new Dictionary<string, TokenType>() {
        { "(", TokenType.LPAR },
        { ")", TokenType.RPAR},
        { "+", TokenType.PLUS },
        { "*", TokenType.MULTIPLICATION },
        { "=", TokenType.EQUAL },
        { @"\n", TokenType.NEWSTRING},
        {",", TokenType.COMMA },
        {"/", TokenType.DIVIDE },
        {"-", TokenType.MINUS }
        };
        public static bool IsSpecialSymbol(string ch)
        {
            return (SpecialSymbols.ContainsKey(ch));
        }
        string allText, temp = "";
        int type;
        public AnalysisCode(string text)
        {
            allText = text;
        }
        public void Res(string temp, int type)
        {
            if (type == 1)
            {
                if (!IsSpecialWord(temp))
                {
                    Token token = new Token(TokenType.INDENTIFIER);
                    token.Value = temp;
                    tokens.Add(token);
                }
                else
                {
                    Token token1 = new Token(Keywords[temp]);
                    tokens.Add(token1);
                }
            }
            if (type == 2)
            {


                Token token = new Token(TokenType.LITERAL);
                token.Value = temp;
                tokens.Add(token);

            }
            if (type == 3)
            {
                if (IsSpecialSymbol(temp))
                {
                    Token token = new Token(SpecialSymbols[temp]);
                    tokens.Add(token);
                }
            }
        }
        public void Show()
        {
            Console.WriteLine("List of tokens:");
            foreach (Token token in tokens)
            {
                Console.WriteLine($"Type: {token.Type}, Value: {token.Value}");
            }
        }
        public void action()
        {
            for (int i = 0; i < allText.Length; i++)
            {
                Analysis(allText[i]);
            }
        }
        public void Analysis(char chCurrent)
        {
           
            
            if (chCurrent >= 'a' && chCurrent <= 'z' || chCurrent >= 'A' && chCurrent <= 'Z')
            {
                if (temp == "")
                {
                    type = 1;
                }
                temp += chCurrent;
                return;
            }

            if ((chCurrent >= '0' && chCurrent <= '9'))
            {
                if (temp == "")
                {
                    type = 2;
                }
                temp += chCurrent;
                return;
            }
            if ((chCurrent == ' ' || chCurrent == '\n') && temp != "")
            {
                
                Res(temp, type);
                if (chCurrent == '\n')
                {

                    Res(@"\n", 3);
                }
                temp = "";

                return;
                
            }
                if (IsSpecialSymbol(chCurrent.ToString()))
                {
                if (temp == "" )
                {
                    type = 3;
                }
                temp += chCurrent;
                return;
                }

        }
    }
}
