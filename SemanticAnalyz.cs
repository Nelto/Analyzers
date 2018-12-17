using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Analyzers
{
    class SemanticAnalyz
    {
        List<string> tn;
        List<string> lexemes;
        bool isNegation = false;
        int num = 0;
        Dictionary<string, string> id = new Dictionary<string, string>();

        public SemanticAnalyz(List<string> tn, List<string> lexemes)
        {
            this.tn = tn;
            this.lexemes = lexemes;
        }

        private void Parse()
        {
            while (lexemes[num] != "114")
            {
                if (Regex.IsMatch(lexemes[num], @"10|11|12")) Description();
                else if (Regex.IsMatch(lexemes[num], @"^(3\d|13)$")) Assignment();
                else if (lexemes[num] == "14") If();
                else if (lexemes[num] == "18") For();
                else if (lexemes[num] == "19" && lexemes[num + 1] == "110") While();
                else if (lexemes[num] == "112" || lexemes[num] == "113") InOutPut();
                else num++;
            }
        }

        private void Description()
        {
            string type = lexemes[num];
            while (true)
            {
                if (Regex.IsMatch(lexemes[num], @"20|26"))
                {
                    num++;
                    break;
                }
                if (id.ContainsKey(lexemes[num])) throw new Exception("Повторное обьявление переменной");
                id.Add(lexemes[num], type);
                num++;
                if (lexemes[num] == "21") num++;
            }
        }

        private void Assignment()
        {
            if (lexemes[num] == "13") num++;
            string type = GetType(lexemes[num]);
            num += 2;
            int start = num;
            while (lexemes[num] != ";" || lexemes[num] != ":") num++;
            int end = num - 1;
            switch (type)
            {
                case "10":
                case "11":
                case "12": BoolExpression(start, end);
                default: 
            }
        }

        private void If()
        {

        }

        private void For()
        {

        }

        private void While()
        {

        }

        private void InOutPut()
        {

        }

        private void IntExpression()
        {

        }

        private void BoolExpression(int start, int end)
        {
            List<string[]> rpe = GetRPE(start, end);
            foreach (string[] lex in rpe)
            {
                if (lex.Length == 1)
                {
                    if (lex[0][0] != '3') throw new Exception("Ожидался тип bool");
                    else if(!(GetType(lex[0]) == "12")) throw new Exception("Операции or, and и ~ не применимы к типам int и float");
                }

                else if (lex.Length == 2)
                {
                    if (!(lex[0][0] == '3' && lex[1] == "220")) throw new Exception("Ожидался тип bool");
                    else if (GetType(lex[0]) == "12") throw new Exception("Операции or, and и ~ не применимы к типам int и float");
                }

                else if (Regex.IsMatch(lex.Last(), @"28|29|210|211|212|213"))
                {
                    for (int i = 0; i < lex.Length - 1; i++)
                    {
                        if (Regex.IsMatch(lex[i], @"28|29|210|211|212|213")) throw new Exception("Ошибка при использовании операции сравнения");
                        if (lex[i] == "220") throw new Exception("Ошибка при использовании операции ~");

                        if (lex[i][0] == '3')
                        {
                            if (GetType(lex[i]) == "12") throw new Exception("Операция не применима к типу bool");
                        }
                    }
                }

                else if (lex.Last() == "220" && Regex.IsMatch(lex.Last(), @"28|29|210|211|212|213"))
                {
                    for (int i = 0; i < lex.Length - 2; i++)
                    {
                        if (Regex.IsMatch(lex[i], @"28|29|210|211|212|213")) throw new Exception("Ошибка при использовании операции сравнения");
                        if (lex[i] == "220") throw new Exception("Ошибка при использовании операции ~");

                        if (lex[i][0] == '3')
                        {
                            if (GetType(lex[i]) == "12") throw new Exception("Операция не применима к типу bool");
                        }
                    }
                }

                else throw new Exception("Ожидался тип bool");
            }
        }

        private string GetType(string lex)
        {
            if (!id.ContainsKey(lex)) throw new Exception("Используется переменная, но она нигде не обьявлена");
            else return id[lex];
        }

        private void floatExpressiom()
        {

        }

        private List<string[]> GetRPE(int start, int end)
        {
            Stack<string> operStack = new Stack<string>();
            List<string[]> g = new List<string[]>();
            string a = "";
            for (int i = start; i < end; i++)
            {
                if (IsOperator(lexemes[i]))
                {
                    if (lexemes[i] == "(")
                        operStack.Push(lexemes[i]);
                    else if (lexemes[i] == ")")
                    {
                        string s = operStack.Pop();

                        while (s != "(")
                        {
                            a += s + " ";
                            s = operStack.Pop();
                        }
                    }
                    else if (lexemes[i] == "or" || lexemes[i] == "and")
                    {
                        while (operStack.Count > 0)
                        {
                            if (operStack.Peek() == "(") break;

                            a += operStack.Pop() + " ";
                        }

                        g.Add(a.Trim().Split(' '));
                        a = "";
                    }
                    else
                    {
                        point:
                        if (operStack.Count > 0)
                            if (GetPriority(lexemes[i]) <= GetPriority(operStack.Peek()))
                            {
                                a += operStack.Pop() + " ";
                                goto point;
                            }


                        operStack.Push(lexemes[i]);
                    }
                }

                else if (!IsOperator(lexemes[i]))
                {
                    a += lexemes[i] + " ";
                }
            }

            while (operStack.Count > 0)
            {
                a += operStack.Pop() + " ";
            }
            g.Add(a.Trim().Split(' '));
            return g;
        }

        private byte GetPriority(string s)
        {
            switch (s)
            {
                case "(": return 0; // '('
                case ")": return 0;// ')'

                case "or": return 1; //or
                case "and": return 1; //and

                case "NE": return 3; //NE
                case "EQ": return 3; //EQ
                case "LT": return 3; //LT
                case "LE": return 3; //LE
                case "GT": return 3; //GT
                case "GE": return 3; //GE

                case "plus": return 4;//plus
                case "min": return 4;//min

                case "mult": return 5;//mult
                case "div": return 5;//div

                case "~": return 6;//~
                default: return 7;
            }
        }

        private bool IsOperator(string oper)
        {
            return (Regex.IsMatch(oper, "plus|min|[(|)]|mult|div|~|NE|EQ|LT|LE|GT|GE|or|and"));
        }
    }
}
