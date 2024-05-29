using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11
{
    public class LR
    {
        List<Token> tokens = new List<Token>();
        Stack<Token> lexemStack = new Stack<Token>();
        Stack<int> stateStack = new Stack<int>();
        private static List<KeyValuePair<string, int>> inputValues = new List<KeyValuePair<string, int>>();
        int nextLex = 0;
        int state = 0;
        bool isEnd = false;
        int count = 0;
        public LR(List<Token> inputtoken)
        {
            tokens = inputtoken;
        }
        private Token GetLexeme(int nextLex)
        {
            return tokens[nextLex];
        }

        private void Shift()
        {
            lexemStack.Push(GetLexeme(nextLex));
            nextLex++;
        }
        private void GoToState(int state)
        {
            stateStack.Push(state);
            this.state = state;
        }
        private void Reduce(int num, string value)
        {
            for (int i = 0; i < num; i++)
            {
                lexemStack.Pop();
                stateStack.Pop();
            }
            state = stateStack.Peek();
            Token k = new Token(TokenType.NETERMINAL);
            k.Value = value;
            lexemStack.Push(k);
        }
        private void ReduceEXPR(int num, string value)
        {
            for (int i = 0; i < num; i++)
            {
                lexemStack.Pop();
                
            }
            stateStack.Pop();
            stateStack.Pop();
            stateStack.Pop();
            state = stateStack.Peek();
            Token k = new Token(TokenType.NETERMINAL);
            k.Value = value;
            lexemStack.Push(k);
        }
        public void Programm()
        {
            Start();
        }


        void State0()
        {
            if (lexemStack.Count == 0)
                Shift();
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<прогр>":
                                isEnd = true;
                            break;
                        case "<список_описаний>":
                            GoToState(1);
                            break;
                        case "<описание>":
                            GoToState(2);
                            break;
                    }
                    break;
                case TokenType.DIM:
                    GoToState(3);
                    break;
                default:
                    throw new Exception($"Ожидалось Dim, но было получено {lexemStack.Peek().ToString()}. State: 0");
            }
        }
        void State1()
        {

            switch (lexemStack.Peek().Type)
            {

                case TokenType.NETERMINAL:

                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_опер>":
                            GoToState(4);
                            break;
                        case "<опер>":
                            GoToState(5);
                            break;
                        case "<присваивание>":
                            GoToState(6);
                            break;
                        case "<услов_конструктор>":
                            GoToState(7);
                            break;
                        case "<описание>":
                            GoToState(10);
                            break;
                        case "<список_описаний>":
                            Shift();
                            break;
                    }
                    break;
                case TokenType.INDENTIFIER:
                    GoToState(8);
                    break;
                case TokenType.SELECT:
                    GoToState(9);
                    break;
                case TokenType.DIM:
                    GoToState(3);
                    break;
                default:
                    throw new Exception($"Ожидалось Dim, id или select, но было получено {lexemStack.Peek().ToString()}. State: 1");
            }
        }
        void State2()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<описание>":
                            Shift();
                            break;
                    }
                    break;
                case TokenType.NEWSTRING:
                    GoToState(11);
                    break;
                default:
                    throw new Exception($"Ожидалось newstring, но было получено {lexemStack.Peek().ToString()}. State: 2");
            }
        }
        void State3()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_переменных>":
                            GoToState(12);
                            break;
                    }
                    break;
                case TokenType.DIM:
                    Shift();
                    break;
                case TokenType.INDENTIFIER:
                    GoToState(13);
                    break;
                default:
                    throw new Exception($"Ожидалось id, но было получено {lexemStack.Peek().ToString()}. State: 3");
            }
        }
        void State4()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        
                        case "<список_опер>":

                            if (GetLexeme(nextLex).Type == TokenType.SELECT || GetLexeme(nextLex).Type == TokenType.INDENTIFIER)
                            {
                                Shift();
                            }
                            else if (lexemStack.Count == tokens.Count)
                            {
                                isEnd = true;
                                break;
                             }
                            else
                            {
                                Reduce(2, "<прогр>");
                                break;
                            }
                            break;
                        case "<опер>":
                            GoToState(14);
                            break;
                        case "<присваивание>":
                            GoToState(6);
                            break;
                        case "<услов_конструктор>":
                            GoToState(7);
                            break;
                    }
                    break;
                case TokenType.INDENTIFIER:
                    GoToState(8);
                    break;
                case TokenType.SELECT:
                    GoToState(9);
                    break;
                default:
                    throw new Exception($"Ожидался id или select но было получено {lexemStack.Peek().ToString()}. State: 4");
            }
        }
        void State5()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<опер>":
                            Shift();
                            break;
                    }
                    break;
                case TokenType.NEWSTRING:
                    GoToState(15);
                    break;
                default:
                    throw new Exception($"Ожидалось newstring, но было получено {lexemStack.Peek().ToString()}. State: 5");
            }
        }
        void State6()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL && lexemStack.Peek().Value == "<присваивание>")
                Reduce(1, "<опер>");
            else
                throw new Exception($"Ожидалось правило <присваивание>, но было получено {lexemStack.Peek().ToString()}. State: 6");
        }
        void State7()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL && lexemStack.Peek().Value == "<услов_конструктор>")
                Reduce(1, "<опер>");
            else
                throw new Exception($"Ожидалось правило <услов_коструктор>, но было получено {lexemStack.Peek().ToString()}. State: 7");
        }
        void State8()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.INDENTIFIER:
                    Shift();
                    break;
                case TokenType.EQUAL:
                    GoToState(16);
                    break;
                default:
                    throw new Exception($"Ожидалось =, но было получено {lexemStack.Peek().ToString()}. State: 8");
            }
        }
        void State9()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SELECT:
                    Shift();
                    break;
                case TokenType.CASE:
                    GoToState(17);
                    break;
                default:
                    throw new Exception($"Ожидалось case, но было получено {lexemStack.Peek().ToString()}. State: 9");
            }
        }
        void State10()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<описание>":
                            Shift();
                            break;
                    }
                    break;
                case TokenType.NEWSTRING:
                    GoToState(18);
                    break;
                default:
                    throw new Exception($"Ожидалось newstring, но было получено {lexemStack.Peek().ToString()}. State: 10");
            }
        }
        void State11()
        {
            if (lexemStack.Peek().Type == TokenType.NEWSTRING)
                Reduce(2, "<список_описаний>");
            else
                throw new Exception($"Ожидалось newstring, но было получено {lexemStack.Peek().ToString()}. State: 11");
        }
        void State12()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_переменных>":
                            Shift();
                            break;
                    }
                    break;
                case TokenType.AS:
                    GoToState(19);
                    break;
                case TokenType.COMMA:
                    GoToState(20);
                    break;
                default:
                    throw new Exception($"Ожидалось as или запятая, но было получено {lexemStack.Peek().ToString()}. State: 12");
            }
        }
        void State13()
        {
            if (lexemStack.Peek().Type == TokenType.INDENTIFIER)
                Reduce(1, "<список_переменных>");
            else
                throw new Exception($"Ожидалось id, но было получено {lexemStack.Peek().ToString()}. State: 12");
        }
        void State14()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<опер>":
                            GoToState(21);
                            break;
                    }
                    break;
                default:
                    throw new Exception($"Ожидалось newstring, но было получено {lexemStack.Peek().ToString()}. State: 14");
            }
        }
        void State15()
        {
            if (lexemStack.Peek().Type == TokenType.NEWSTRING)
                Reduce(2, "<список_опер>");
            else
                throw new Exception($"Ожидалось newstring, но было получено {lexemStack.Peek().ToString()}. State: 15");
        }
        void State16()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.EQUAL:
                    Shift();
                    break;
                case TokenType.INDENTIFIER:
                    while (GetLexeme(nextLex).Type != TokenType.NEWSTRING) { count++;Shift(); }
                    GoToState(22);
                    break;
                case TokenType.LITERAL:
                    while (GetLexeme(nextLex).Type != TokenType.NEWSTRING) { count++; Shift(); }
                    GoToState(22);
                    break;
                default:
                    throw new Exception($"Ожидалось expr, но было получено {lexemStack.Peek().ToString()}. State: 16");
            }
        }

        void State17()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.CASE:
                    Shift();
                    break;
                case TokenType.INDENTIFIER:
                    GoToState(23);
                    break;
                default:
                    throw new Exception($"Ожидалось id, но было получено {lexemStack.Peek().ToString()}. State: 17");
            }
        }
        void State18()
        {
            if (lexemStack.Peek().Type == TokenType.NEWSTRING)
                Reduce(3, "<список_описаний>");
            else
                throw new Exception($"Ожидалось newstring, но было получено {lexemStack.Peek().ToString()}. State: 18");
        }
        void State19()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<тип>":
                            GoToState(24);
                            break;
                    }
                    break;
                case TokenType.AS:
                    Shift();
                    break;
                case TokenType.INTEGER:
                    GoToState(25);
                    break;
                case TokenType.BOOLEAN:
                    GoToState(26);
                    break;
                case TokenType.BYTE:
                    GoToState(27);
                    break;
                default:
                    throw new Exception($"Ожидалось integer, boolean или byte, но было получено {lexemStack.Peek().ToString()}. State: 19");
            }
        }
        void State20()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.COMMA:
                    Shift();
                    break;
                case TokenType.INDENTIFIER:
                    GoToState(28);
                    break;
                default:
                    throw new Exception($"Ожидалось id, но было получено {lexemStack.Peek().ToString()}. State: 20");
            }
        }
        void State21()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL)
                Reduce(2, "<список_опер>");
            else
                throw new Exception($"Ожидалось newstring, но было получено {lexemStack.Peek().ToString()}. State: 21");
        }
        void State22()
        {
            if(lexemStack.Peek().Type != TokenType.NEWSTRING) {
                ReduceEXPR(3 + count, "<присваивание>");
                count = 0;
            }
            else
            {
                throw new Exception($"Ожидалось expr, но было получено {lexemStack.Peek().ToString()}. State: 22");
            }
                
        }
        void State23()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.INDENTIFIER:
                    Shift();
                    break;
                case TokenType.NEWSTRING:
                    GoToState(29);
                    break;
                default:
                    throw new Exception($"Ожидалось newstring, но было получено {lexemStack.Peek().ToString()}. State: 23");
            }
        }
        void State24()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL && lexemStack.Peek().Value == "<тип>")
                Reduce(4, "<описание>");
            else
                throw new Exception($"Ожидалось правило <тип>, но было получено {lexemStack.Peek().ToString()}. State: 24");
        }
        void State25()
        {
            if (lexemStack.Peek().Type == TokenType.INTEGER)
                Reduce(1, "<тип>");
            else
                throw new Exception($"Ожидалось integer, но было получено {lexemStack.Peek().ToString()}. State: 25");
        }
        void State26()
        {
            if (lexemStack.Peek().Type == TokenType.BOOLEAN)
                Reduce(1, "<тип>");
            else
                throw new Exception($"Ожидалось boolean, но было получено {lexemStack.Peek().ToString()}. State: 26");
        }
        void State27()
        {
            if (lexemStack.Peek().Type == TokenType.BYTE)
                Reduce(1, "<тип>");
            else
                throw new Exception($"Ожидалось byte, но было получено {lexemStack.Peek().ToString()}. State: 27");
        }
        void State28()
        {
            if (lexemStack.Peek().Type == TokenType.INDENTIFIER)
                Reduce(3, "<список_переменных>");
            else
                throw new Exception($"Ожидалось id, но было получено {lexemStack.Peek().ToString()}. State: 28");
        }
        void State29()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_условий>":
                            GoToState(30);
                            break;
                        case "<условие>":
                            GoToState(31);
                            break;
                    }
                    break;
                case TokenType.NEWSTRING:
                    Shift();
                    break;
                case TokenType.CASE:
                    GoToState(32);
                    break;
                default:
                    throw new Exception($"Ожидалось case, но было получено {lexemStack.Peek().ToString()}. State: 29");
            }
        }
        void State30()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_условий>":
                            Shift();
                            break;
                    }
                    break;
                case TokenType.CASE:
                    GoToState(32);
                    break;
                case TokenType.END:
                    GoToState(36);
                    break;
                default:
                    throw new Exception($"Ожидалось case или end, но было получено {lexemStack.Peek().ToString()}. State: 30");
            }
        }
        void State31()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL && lexemStack.Peek().Value == "<условие>")
                Reduce(1, "<список_условий>");
            else
                throw new Exception($"Ожидалось правило <условие>, но было получено {lexemStack.Peek().ToString()}. State: 31");

        }
        void State32()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.CASE:
                    Shift();
                    break;
                case TokenType.LITERAL:
                    GoToState(34);
                    break;
                case TokenType.ELSE:
                    GoToState(35);
                    break;
                default:
                    throw new Exception($"Ожидалось else или literal, но было получено {lexemStack.Peek().ToString()}. State: 32");
            }
        }
        void State33()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.END:
                    GoToState(36);
                    break;
                default:
                    throw new Exception($"Ожидалось end, но было получено {lexemStack.Peek().ToString()}. State: 33");
            }
        }
        void State34()
        {
            switch (lexemStack.Peek().Type)
            {

                case TokenType.LITERAL:
                    Shift();
                    break;
                case TokenType.NEWSTRING:
                    GoToState(37);
                    break;
                case TokenType.TO:
                    GoToState(38);
                    break;
                default:
                    throw new Exception($"Ожидалось to или newstring, но было получено {lexemStack.Peek().ToString()}. State: 34");
            }
        }
        void State38()
        {
            switch (lexemStack.Peek().Type)
            {

                case TokenType.TO:
                    Shift();
                    break;
                case TokenType.LITERAL:
                    GoToState(42);
                    break;
                default:
                    throw new Exception($"Ожидалось literal, но было получено {lexemStack.Peek().ToString()}. State: 38");
            }
        }
        void State35()
        {
            switch (lexemStack.Peek().Type)
            {

                case TokenType.ELSE:
                    Shift();
                    break;
                case TokenType.NEWSTRING:
                    GoToState(39);
                    break;
                default:
                    throw new Exception($"Ожидалось newstring, но было получено {lexemStack.Peek().ToString()}. State: 35");
            }
        }
        void State36()
        {
            switch (lexemStack.Peek().Type)
            {

                case TokenType.END:
                    Shift();
                    break;
                case TokenType.SELECT:
                    GoToState(40);
                    break;
                default:
                    throw new Exception($"Ожидалось select, но было получено {lexemStack.Peek().ToString()}. State: 36");
            }
        }
        void State37()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_опер>":
                            GoToState(41);
                            break;
                        case "<опер>":
                            GoToState(5);
                            break;
                        case "<присваивание>":
                            GoToState(6);
                            break;
                        case "<услов_конструктор>":
                            GoToState(7);
                            break;
                    }
                    break;
                case TokenType.NEWSTRING:
                    Shift();
                    break;
                case TokenType.SELECT:
                    GoToState(9);
                    break;
                case TokenType.INDENTIFIER:
                    GoToState(8);
                    break;
                default:
                    throw new Exception($"Ожидалось id или select, но было получено {lexemStack.Peek().ToString()}. State: 37");
            }
        }
        void State42()
        {
            switch (lexemStack.Peek().Type)
            {

                case TokenType.LITERAL:
                    Shift();
                    break;
                case TokenType.NEWSTRING:
                    GoToState(44);
                    break;
                default:
                    throw new Exception($"Ожидалось newstring, но было получено {lexemStack.Peek().ToString()}. State: 42");
            }
        }
        void State39()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_опер>":
                            GoToState(43);
                            break;
                        case "<опер>":
                            GoToState(5);
                            break;
                        case "<присваивание>":
                            GoToState(6);
                            break;
                        case "<услов_коструктор>":
                            GoToState(7);
                            break;
                    }
                    break;
                case TokenType.NEWSTRING:
                    Shift();
                    break;
                case TokenType.SELECT:
                    GoToState(9);
                    break;
                case TokenType.INDENTIFIER:
                    GoToState(8);
                    break;
                default:
                    throw new Exception($"Ожидалось id или select, но было получено {lexemStack.Peek().ToString()}. State: 39");
            }
        }

        void State40()
        {
            if (lexemStack.Peek().Type == TokenType.SELECT)
                Reduce(7, "<услов_конструктор>");
            else
                throw new Exception($"Ожидалось select, но было получено {lexemStack.Peek().ToString()}. State: 40");
        }
        void State41()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_опер>":

                            if (GetLexeme(nextLex).Type == TokenType.SELECT || GetLexeme(nextLex).Type == TokenType.INDENTIFIER)
                            {
                                Shift();

                            }
                            else
                            {
                                Reduce(4, "<условие>");

                            }
                            break;
                        case "<опер>":
                            GoToState(5);
                            break;
                        case "<присваивание>":
                            GoToState(6);
                            break;
                        case "<услов_коструктор>":
                            GoToState(7);
                            break;
                    }
                    break;
                case TokenType.INDENTIFIER:
                    GoToState(8);
                    break;
                case TokenType.SELECT:
                    GoToState(9);
                    break;
                default:
                    throw new Exception($"Ожидался id или select, но было получено {lexemStack.Peek().ToString()}. State: 41");
            }
        }
        void State44()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_опер>":
                            GoToState(45);
                            break;
                        case "<опер>":
                            GoToState(5);
                            break;
                        case "<присваивание>":
                            GoToState(6);
                            break;
                        case "<услов_коструктор>":
                            GoToState(7);
                            break;
                    }
                    break;
                case TokenType.NEWSTRING:
                    Shift();
                    break;
                case TokenType.SELECT:
                    GoToState(9);
                    break;
                case TokenType.INDENTIFIER:
                    GoToState(8);
                    break;
                default:
                    throw new Exception($"Ожидалось id или select, но было получено {lexemStack.Peek().ToString()}. State: 44");
            }
        }
        void State43()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_опер>":

                            if (GetLexeme(nextLex).Type == TokenType.SELECT || GetLexeme(nextLex).Type == TokenType.INDENTIFIER)
                            {
                                Shift();
                            }
                            else
                            {
                                Reduce(5, "<условие>");

                            }
                            break;
                        case "<опер>":
                            GoToState(5);
                            break;
                        case "<присваивание>":
                            GoToState(6);
                            break;
                        case "<услов_коструктор>":
                            GoToState(7);
                            break;
                    }
                    break;
                case TokenType.INDENTIFIER:
                    GoToState(8);
                    break;
                case TokenType.SELECT:
                    GoToState(9);
                    break;
                default:
                    throw new Exception($"Ожидался id или select, но было получено {lexemStack.Peek().ToString()}. State: 43");
            }
        }
        void State45()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_опер>":

                            if (GetLexeme(nextLex).Type == TokenType.SELECT || GetLexeme(nextLex).Type == TokenType.INDENTIFIER)
                            {
                                Shift();

                            }
                            else
                            {
                                Reduce(7, "<условие>");

                            }
                            break;
                        case "<опер>":
                            GoToState(5);
                            break;
                        case "<присваивание>":
                            GoToState(6);
                            break;
                        case "<услов_коструктор>":
                            GoToState(7);
                            break;
                    }
                    break;
                case TokenType.INDENTIFIER:
                    GoToState(8);
                    break;
                case TokenType.SELECT:
                    GoToState(9);
                    break;
                default:
                    throw new Exception($"Ожидался id или select, но было получено {lexemStack.Peek().ToString()}. State: 45");
            }
        }
        public void Start()
        {
            stateStack.Push(0);
            while (isEnd != true)
            {
                switch (state)
                {
                    case 0:
                        State0();
                        break;
                    case 1:
                        State1();
                        break;
                    case 2:
                        State2();
                        break;
                    case 3:
                        State3();
                        break;
                    case 4:
                        State4();
                        break;
                    case 5:
                        State5();
                        break;
                    case 6:
                        State6();
                        break;
                    case 7:
                        State7();
                        break;
                    case 8:
                        State8();
                        break;
                    case 9:
                        State9();
                        break;
                    case 10:
                        State10();
                        break;
                    case 11:
                        State11();
                        break;
                    case 12:
                        State12();
                        break;
                    case 13:
                        State13();
                        break;
                    case 14:
                        State14();
                        break;
                    case 15:
                        State15();
                        break;
                    case 16:
                        State16();
                        break;
                    case 17:
                        State17();
                        break;
                    case 18:
                        State18();
                        break;
                    case 19:
                        State19();
                        break;
                    case 20:
                        State20();
                        break;
                    case 21:
                        State21();
                        break;
                    case 22:
                        State22();
                        break;
                    case 23:
                        State23();
                        break;
                    case 24:
                        State24();
                        break;
                    case 25:
                        State25();
                        break;
                    case 26:
                        State26();
                        break;
                    case 27:
                        State27();
                        break;
                    case 28:
                        State28();
                        break;
                    case 29:
                        State29();
                        break;
                    case 30:
                        State30();
                        break;
                    case 31:
                        State31();
                        break;
                    case 32:
                        State32();
                        break;
                    case 33:
                        State33();
                        break;
                    case 34:
                        State34();
                        break;
                    case 35:
                        State35();
                        break;
                    case 36:
                        State36();
                        break;
                    case 37:
                        State37();
                        break;
                    case 38:
                        State38();
                        break;
                    case 39:
                        State39();
                        break;
                    case 40:
                        State40();
                        break;
                    case 41:
                        State41();
                        break;
                    case 42:
                        State42();
                        break;
                    case 43:
                        State43();
                        break;
                    case 44:
                        State44();
                        break;
                    case 45:
                        State45();
                        break;
                }
            }
        }

    }
}
