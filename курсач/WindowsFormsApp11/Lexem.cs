using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WindowsFormsApp11 
{
    class BetaTab
    {
        string[,] buff = new string[2, 255];
        
        int j = 0;
        string Alfavit = @"=<>()+-*/\n,";
        public string[,] Buff { get { return buff; } }

        bool Cheking()
        {
            for (int i = 0; i <= j; i++)
            {
                bool resultinteger = int.TryParse(buff[0, i], out _);
                bool resultdouble = double.TryParse(buff[0, i], out _);
                if (buff[0, i] == null) { break; }
                if (buff[0, i].Length > 8) return false;
                else if (Regex.IsMatch(buff[0, i], @"^[a-zA-Z]+$")) buff[1, i] = "id";
                else if (resultinteger || resultdouble) buff[1, i] = "literal";
                else if (Alfavit.Contains(buff[0, i])) buff[1, i] = "delimiter";
                else return false;
            }
            return true;
        }
        bool Add(string str)
        {
            string ch = "";
            for (int i = 0; str.Length > i; i++)
            {
                if (str[i] == ' ' || str[i] == '\r')
                {
                    AddElem(ch);
                    ch = "";
                    i++;
                }
                if (str[i] == '\n')
                { AddElem(@"\n"); i++; if (i >= str.Length) break; }
                while (str[i] == '\t')
                { i++; if (i >= str.Length) break; }
                ch += str[i];
                if (ch == " ") { ch = ""; }
            }
            AddElem(ch);
            return Cheking();
        }
        void AddElem(string str)
        {
            if (str == "")
            {
                return;
            }
            buff[0, j] += str;
            j++;
            return;
        }
        public string Info(string str)
        {
            if (!Add(str)) { buff = null; throw new Exception("Недопустимый символ"); }
            string info = "";
            for (int i = 0; buff[1, i] != null; i++)
            {
                info += buff[0, i] + ", " + buff[1, i] + "\r" + "\n";
            }
            return info;
        }
    }
}

