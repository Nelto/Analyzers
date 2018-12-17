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
        int num = 0;
        Dictionary<string, string> id = new Dictionary<string, string>();

        public SemanticAnalyz(List<string> tn, List<string> parser)
        {
            this.tn = tn;
            lexemes = parser;
            Recognition();
        }

        private void Recognition()
        {
            while (lexemes[num] != "114")
            {
                if (Regex.IsMatch(lexemes[num], @"^(10|11|12)$")) Description();
                else if (Regex.IsMatch(lexemes[num], @"^(3\d|13)$")) Assignment();
                else if (lexemes[num] == "14") If();
                else if (lexemes[num] == "18") For();
                else if (lexemes[num] == "19" && lexemes[num + 1] == "110") While();
                else if (lexemes[num] == "112") InPut();
                else if (lexemes[num] == "113") OutPut();
                else num++;
            }
            Console.WriteLine("Семантический анализ успешной пройден");
        }

        private void Description()
        {
            string type = lexemes[num];
            num++;
            while (true)
            {
                if (Regex.IsMatch(lexemes[num], @"^(20|26)$"))
                {
                    num++;
                    break;
                }
                if (id.ContainsKey(lexemes[num])) throw new Exception("Код ошибки 12: "+ Exceptions.GetExcep(12));
                id.Add(lexemes[num], type);
                num++;
                if (lexemes[num] == "21") num++;
            }
        }

        private void Assignment()
        {
            int err = -1;
            if (lexemes[num] == "13") num++;
            string type = GetType(lexemes[num]);
            num += 2;
            int start = num;
            while (!(lexemes[num] == "20" || lexemes[num] == "26")) num++;
            int end = num;
            switch (type)
            {
                case "10":
                    err = IntExpression(start, end);
                    break;
                case "11":
                    err = FloatExpressiom(start, end);
                    break;
                case "12":
                    err = BoolExpression(start, end);
                    break;
            }
            if (err > -1) throw new Exception("Код ошибки " + err + ": " + Exceptions.GetExcep(err));
        }

        private void If()
        {
            int err = -1;
            num++;
            int start = num;
            while (lexemes[num] != "15") num++;
            err = BoolExpression(start, num);
            if (err > -1) throw new Exception("Код ошибки " + err + ": " + Exceptions.GetExcep(err));
        }

        private void For()
        {
            int interr = -1;
            int floaterr = -1;
            int boolerr = -1;
            num += 2;
            int start = num;
            if (!(lexemes[num] == "26"))
            {
                while (!(lexemes[num] == "26")) num++;
                interr = IntExpression(start, num);
                floaterr = FloatExpressiom(start, num);
                if (interr > -1 && floaterr > -1 && boolerr > -1) throw new Exception("Код ошибки " + 20 + ": " + Exceptions.GetExcep(20));
            }

            start = num++;
            if (!(lexemes[num] == "26"))
            {
                while (!(lexemes[num] == "26")) num++;
                boolerr = BoolExpression(start, num);
                if (boolerr > -1) throw new Exception("Код ошибки " + boolerr + ": " + Exceptions.GetExcep(boolerr));
            }

            start = num++;
            if (!(lexemes[num] == "25"))
            {
                while (!(lexemes[num][0] == '1' || (lexemes[num][0] == '3' && lexemes[num + 1] == "27"))) num++;
                interr = IntExpression(start, num);
                floaterr = FloatExpressiom(start, num);
                if (interr > -1 && floaterr > -1 && boolerr > -1) throw new Exception("Код ошибки " + 20 + ": " + Exceptions.GetExcep(20));
            }

        }

        private void While()
        {
            int err = -1;
            num += 2;
            int start = num;
            while (!(lexemes[num][0] == '1' || (lexemes[num][0] == '3' && lexemes[num + 1] == "27")))
            {
                num++;
            }
            err = BoolExpression(start, num);
            if (err > -1) throw new Exception("Код ошибки " + err + ": " + Exceptions.GetExcep(err));
        }

        private void InPut()
        {
            num += 2;
            while (lexemes[num] != "25")
            {
                GetType(lexemes[num]);
                num++;
            }
        }

        private void OutPut()
        {
            int interr = -1;
            int floaterr = -1;
            int boolerr = -1;
            num += 2;
            int start = num;
            while (!Regex.IsMatch(lexemes[num], @"^(20|26)$"))
            {
                if (lexemes[num] == "222")
                {
                    interr = IntExpression(start, num);
                    floaterr = FloatExpressiom(start, num);
                    boolerr = BoolExpression(start, num);
                    if (interr > -1 && floaterr > -1 && boolerr > -1) throw new Exception("Код ошибки " + 19 + ": " + Exceptions.GetExcep(19));
                    start = num++;
                }
                num++;
            }

            interr = IntExpression(start, num-1);
            floaterr = FloatExpressiom(start, num-1);
            boolerr = BoolExpression(start, num-1);
            if (interr > -1 && floaterr > -1 && boolerr > -1) throw new Exception("Код ошибки " + 19 + ": " + Exceptions.GetExcep(19));
        }

        private int IntExpression(int start, int end)
        {
            int err;
            int numberInTN;
            for (int i = start; i < end; i++)
            {
                err = InvalidStatement(lexemes[i]);
                if (err > -1) return err;
                
                if (lexemes[i][0] == '4')
                {
                    numberInTN = Convert.ToInt32(lexemes[i].Remove(0, 1));
                    if (Regex.IsMatch(tn[numberInTN], "[0-9]*[.][0-9]+([Ee][+-]?[0-9]+)?|[0-9]+[Ee][+-]?[0-9]+")) return 18;
                }

                if(lexemes[i][0] == '3')
                 if (GetType(lexemes[i]) != "10") return 18;
            }
            return -1;
        }

        private int FloatExpressiom(int start, int end)
        {
            int err;
            for (int i = start; i < end; i++)
            {
                err = InvalidStatement(lexemes[i]);
                if (err > -1) return err;
            }
            return -1;
        }

        private int BoolExpression(int start, int end)
        {
            int err;
            List<string[]> rpe = GetRPE(start, end);
            foreach (string[] lex in rpe)
            {
                if (lex.Length == 1)
                {
                    if (Regex.IsMatch(lex[0], @"^(115|116)$")) break;
                    if (!(lex[0][0]=='3')) return 14;
                    else if (!(GetType(lex[0]) == "12")) return 15;
                }

                else if (lex.Length == 2)
                {
                    if (!(Regex.IsMatch(lex[0], @"^(3\d|115|116)$") && lex[1] == "220")) return 14;
                    else if (GetType(lex[0]) == "12") return 15;
                }

                else if (Regex.IsMatch(lex.Last(), @"^(28|29|210|211|212|213)$"))
                {
                    for (int i = 0; i < lex.Length - 1; i++)
                    {
                        err = InvalidStatement(lex[i]);
                        if (err > -1) return err;
                    }
                }

                else if (lex.Last() == "220" && Regex.IsMatch(lex.Last(), @"^(28|29|210|211|212|213)$"))
                {
                    for (int i = 0; i < lex.Length - 2; i++)
                    {
                        err = InvalidStatement(lex[i]);
                        if (err > -1) return err;
                    }
                }
                else return 14;
            }
            return -1;
        }

        private string GetType(string lex)
        {
            if (!id.ContainsKey(lex)) throw new Exception("Код ошибки 13:" + Exceptions.GetExcep(13));
            else return id[lex];

        }

        private int InvalidStatement(string lex)
        {
            if (Regex.IsMatch(lex, @"^(28|29|210|211|212|213)$")) return 17;
            if (Regex.IsMatch(lex, @"^(218|219|220)$")) return 15;
            if (lex[0] == '3')
            {
                if (GetType(lex) == "12") return 16;
            }
            if (Regex.IsMatch(lex, @"^(115|116$)")) return 16;
            return -1;
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
                    if (lexemes[i] == "24")
                        operStack.Push(lexemes[i]);
                    else if (lexemes[i] == "25")
                    {
                        string s = operStack.Pop();

                        while (s != "24")
                        {
                            a += s + " ";
                            s = operStack.Pop();
                        }
                    }
                    else if (lexemes[i] == "219" || lexemes[i] == "218")
                    {
                        while (operStack.Count > 0)
                        {
                            if (operStack.Peek() == "24") break;

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
                case "24": return 0; //'('
                case "25": return 0; //')'

                case "219": return 1; //or
                case "218": return 1; //and

                case "28": return 3; //NE
                case "29": return 3; //EQ
                case "210": return 3; //LT
                case "211": return 3; //LE
                case "212": return 3; //GT
                case "213": return 3; //GE

                case "214": return 4; //plus
                case "215": return 4; //min

                case "216": return 5; //mult
                case "217": return 5; //div

                case "220": return 6; //~
                default: return 7;
            }
        }

        private bool IsOperator(string oper)
        {
            return (Regex.IsMatch(oper, "^(24|25|219|218|28|29|210|211|212|214|216|217|220)$"));
        }
    }
}
