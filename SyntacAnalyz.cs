using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Analyzers
{
    class SintacAnalyz
    {
        string[] lexemes;
        int num = 0; //шаг
        int bracesCount = 0;  //для подсчета {}
        int parantheses = 0;  //для подсчета ()

        public SintacAnalyz(string lexer)
        {
            lexemes = lexer.Split(' ');
            if (lexemes[lexemes.Length] != "114") throw new Exception("Ожидался end");
        }

        public void Parse()
        {
            for (; num < lexemes.Length - 1;)
            {
                if (Regex.IsMatch(lexemes[num], @"^10|11|12$")) Description();
                else Operator();
            }
            if (bracesCount != 0) throw new Exception("Не хватает фигурных скобок(о боже!)");
        }

        private void Operator()
        {
            if (Regex.IsMatch(lexemes[num], @"^3\d|13$")) Assignment();
            else if (lexemes[num] == "14") If();
            else if (lexemes[num] == "18") For();
            else if (lexemes[num] == "19" && lexemes[num + 1] == "110") While();
            else if (lexemes[num] == "112") Input();
            else if (lexemes[num] == "113") Output();
            else if (lexemes[num] == "22")
            {
                bracesCount++;
                num++;
            }
            else if (lexemes[num] == "23")
            {
                bracesCount--;
                num++;
            }
            else if (lexemes[num] == "225" && lexemes[num + 1] == "225") num += 2;
            else throw new Exception("Пока не придумал");
        }

        //описание типа
        private void Description()
        {
            num++;
            if (lexemes[num][0] == '3') num++;
            else throw new Exception("Ошибка в описаниее данных");
            while (true)
            {
                if (lexemes[num] == "20")
                {
                    num++;
                    break;
                }
                else if (lexemes[num] == "21" && lexemes[num][0] == '3') num += 2;
                else throw new Exception("Ошибка в описаниее данных");
            }
        }

        //присваивание
        private void Assignment()
        {
            if (lexemes[num] == "13") num++;
            if (lexemes[num][0] == '3' && lexemes[num + 1] == "27")
            {
                num += 2;
                parantheses = 0;
                Expression(false);
            }
            else throw new Exception("Хьюстон, у нас проблемы");
        }

        private void If()
        {
            num++;
            parantheses = 0;
            Expression(false);
            if (lexemes[num] == "15")
            {
                num++;
            }
            else throw new Exception("А где then?");

            while (true)
            {
                if (lexemes[num] == "17")
                {
                    num++;
                    break;
                }
                else if (lexemes[num] == "16")
                {
                    num++;
                    Operator();
                }
                else throw new Exception("Что-то не так с условием");
            }
        }

        private void For()
        {
            num++;
            if (lexemes[num] == "24") num++;
            else throw new Exception("Ошибкааа");

            if (lexemes[num] == "26") num++;
            else
            {
                parantheses = 0;
                Expression(true);
            }

            if (lexemes[num] == "26") num++;
            else
            {
                parantheses = 0;
                Expression(true);
            }

            if (lexemes[num] == "25") num++;
            else
            {
                parantheses = 1;
                Expression(false);
            }
        }

        private void While()
        {
            num += 2;
            parantheses = 0;
            Expression(false);
            while (true)
            {
                if (lexemes[num] == "111")
                {
                    num++;
                    break;
                }
                Operator();
            }
        }

        private void Input()
        {

        }

        private void Output()
        {

        }

        /*выражение. 24 - "("  25 - ")" 3\d и 4\d соответствуют таблицам идентификаторов и чисел
                     224 - "~" 26 - ";" 20,22,23,211 - ":","{","}"," " соответственно,
                     1\d - таблица служебных слов 27 - "=" 212-223 - диапозон действий
        */
        private void Expression(bool semicolon)
        {
            while (true)
            {
                while (true)
                {
                    if (lexemes[num] == "224") num++;
                    else if (lexemes[num] == "24")
                    {
                        num++;
                        parantheses++;
                    }
                    else break;
                }

                if (Regex.IsMatch(lexemes[num], @"^3\d|4\d$"))
                {
                    num++;
                }
                else throw new Exception("Ошибка в выражении");

                while (true)
                {
                    if (lexemes[num] == "25")
                    {
                        num++;
                        parantheses--;
                    }
                    else break;
                }

                if (semicolon && lexemes[num] == "26")
                {
                    num++;
                    break;
                }
                else if (!semicolon)
                {
                    if (Regex.IsMatch(lexemes[num], @"^1\d|20|22|23|211$") || (lexemes[num][0] == '3' && lexemes[num + 1] == "27"))
                    {
                        break;
                    }
                }
                else if (Regex.IsMatch(lexemes[num], "^[212-223]$")) num++;
                else throw new Exception("Ошибка в выражении");
            }

            if (parantheses != 0) throw new Exception("Ошибка в скобочках");
        }
    }
}
