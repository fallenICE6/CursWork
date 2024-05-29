using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11
{
    internal class BauerZamelzon
    {
        List<Token> tokens;
        string str = "";
        bool flag = true;
        int i = 0;
        int counte = 0;
        Stack<Token> temp = new Stack<Token>();
        Stack<Token> E = new Stack<Token>();
        Stack<Token> T = new Stack<Token>();

        public BauerZamelzon(List<Token> tokens)
        {
            this.tokens = tokens;
            while (i < tokens.Count - 1)
            {
                i++;
                if (tokens[i].Type == TokenType.EQUAL)
                {
                    i++;
                    Expr();
                    flag = true;
                    //str += Matr() + "\r\n";
                }
            }
        }
        void K()
        {
            if (E.Count <= 1)
            {
                throw new Exception("Стек пуст");
            }
            temp.Push(E.Pop());
            temp.Push(E.Pop());
            while (temp.Count > 0)
            {
                E.Push(temp.Pop());
            }
        }
        void Expr()
        {
            while (flag)
            {
                switch (tokens[i].Type)
                {
                    case TokenType.INDENTIFIER:
                        E.Push(tokens[i++]);
                        counte++;
                        break;
                    case TokenType.LITERAL:
                        E.Push(tokens[i++]);
                        counte++;
                        break;

                    case TokenType.NEWSTRING:
                        if (T.Count == 0)
                        {
                            flag = false;
                        }
                        else if (T.Peek().Type == TokenType.LPAR)
                        {
                            throw new Exception("2");
                        }
                        else if(T.Peek().Type == TokenType.PLUS || T.Peek().Type == TokenType.MINUS 
                            || T.Peek().Type == TokenType.MULTIPLICATION || T.Peek().Type == TokenType.DIVIDE)
                        {
                            temp.Push(T.Pop());
                            K();
                        }
                        else
                        {
                            throw new Exception("Ошибка в выражении!");
                        }
                        break;
                    case TokenType.LPAR:
                        if(T.Peek().Type == TokenType.PLUS || T.Peek().Type == TokenType.MINUS 
                            || T.Peek().Type == TokenType.MULTIPLICATION || T.Peek().Type == TokenType.DIVIDE || T.Peek().Type == TokenType.LPAR || T.Count == 0)
                        {
                            T.Push(tokens[i++]);
                        }
                        else
                        {
                            throw new Exception("Ошибка в выражении!");
                        }
                        break;
                    case TokenType.RPAR:
                        if (T.Count == 0)
                        {
                            throw new Exception("Ошибка в выражении!");
                        }
                        else if (T.Peek().Type == TokenType.RPAR)
                        {
                            T.Pop();
                            i++;
                        }
                        else if (T.Peek().Type == TokenType.PLUS || T.Peek().Type == TokenType.MINUS
                            || T.Peek().Type == TokenType.MULTIPLICATION || T.Peek().Type == TokenType.DIVIDE)
                        {
                            temp.Push(T.Pop());
                            K();
                        }
                        break;
                    default:
                        if (tokens[i].Type == TokenType.PLUS || tokens[i].Type == TokenType.MINUS)
                        {
                            if (T.Count == 0)
                            {
                                T.Push(tokens[i++]);
                            }
                            else if (T.Peek().Type == TokenType.PLUS || T.Peek().Type == TokenType.MINUS)
                            {
                                temp.Push(T.Pop());
                                K();
                                T.Push(tokens[i++]);
                            }
                            else if (T.Peek().Type == TokenType.MULTIPLICATION || T.Peek().Type == TokenType.DIVIDE)
                            {
                                temp.Push(T.Pop());
                                K();
                            }
                            else
                            {
                                throw new Exception("abd");
                            }
                        }
                        else if (tokens[i].Type == TokenType.MULTIPLICATION
                        || tokens[i].Type == TokenType.DIVIDE)
                        {
                            if (T.Count == 0 || T.Peek().Type == TokenType.PLUS
                            || T.Peek().Type == TokenType.MINUS)
                            {
                                T.Push(tokens[i++]);
                            }
                            else if (T.Peek().Type == TokenType.MULTIPLICATION || T.Peek().Type == TokenType.DIVIDE)
                            {
                                temp.Push(T.Pop());
                                K();
                                T.Push(tokens[i++]);
                            }
                            else
                            {
                                throw new Exception("2");
                            }
                        }
                        else
                        {
                            throw new Exception("1");
                        }
                        break;
                }
            }
        }
        //string Matr()
        //{
        //    while (E.Count > 0)
        //    {
        //        tmp.Push(E.Pop());
        //        if (tmp.Peek().Type != TokenType.ID && tmp.Peek().Type != TokenType.LITERAL) { count++; }
        //    }
        //    if (counte - count != 1)
        //    {
        //        throw new Exception("Отсутствует логический оператор или операнд");
        //    }
        //    string str = "", v;
        //    string[] s = new string[tmp.Count];
        //    Token t;
        //    int i = 1, j = 0;
        //    while (tmp.Count > 0)
        //    {
        //        t = tmp.Pop();
        //        if (t.Type == TokenType.LESS || t.Type == TokenType.MORE || t.Type == TokenType.AND
        //            || t.Type == TokenType.OR)
        //        {
        //            if (j == 1)
        //            {
        //                throw new Exception("Отсутствует операнд");
        //            }
        //            if (t.Type == TokenType.LESS)
        //            {
        //                v = "M" + i.ToString() + ": " + "<" + s[j - 2] + s[j - 1];
        //            }
        //            else if (t.Type == TokenType.MORE)
        //            {
        //                v = "M" + i.ToString() + ": " + ">" + s[j - 2] + s[j - 1];
        //            }
        //            else
        //            {
        //                v = "M" + i.ToString() + ": " + t.Type.ToString() + s[j - 2] + s[j - 1];
        //            }
        //            s[j - 2] = "M" + i++.ToString();
        //            s[j - 1] = null;
        //            j--;
        //            str += v + "\r\n";
        //        }
        //        else
        //        { s[j++] = t.Value; }
        //    }
        //    return str;
        //}
        //public string Info()
        //{
        //    return str;
        //}
    }
    }
