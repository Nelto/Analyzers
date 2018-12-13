using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Analyzers
{
    class SyntacAnalyz
    {
        List<string> lexemes;
        int num = 0; //шаг
        int bracesCount = 0;  //для подсчета {}
        int parantheses = 0;  //для подсчета ()
        bool fr = false;

        public SyntacAnalyz(List<string> lexer)
        {
            lexemes = lexer;
            if (lexemes[lexemes.Count - 1] != "114") throw new Exception("Ожидался end");
            Parse();
        }

        private void Parse()
        {
            while (num < lexemes.Count - 1)
            {
                if (Regex.IsMatch(lexemes[num], @"^(10|11|12)$")) Description();
                else Operator();
            }
            if (bracesCount != 0) throw new Exception("Не хватает фигурных скобок(о боже!)");
            Console.WriteLine("Все ок");
        }

        private void Operator()
        {
            while (lexemes[num] == "22")
            {
                bracesCount++;
                num++;
            }

            if (Regex.IsMatch(lexemes[num], @"^(3\d|13)$")) Assignment();
            else if (lexemes[num] == "14") If();
            else if (lexemes[num] == "18") For();
            else if (lexemes[num] == "19" && lexemes[num + 1] == "110") While();
            else if (lexemes[num] == "112" || lexemes[num] == "113") InOutPut();
            else throw new Exception("Пока не придумал");

            while (lexemes[num] == "23")
            {
                bracesCount--;
                num++;
            }
        }

        //описание типа
        private void Description()
        {
            num++;
            if (lexemes[num][0] == '3') num++;
            else throw new Exception("Ошибка в описаниее данных");
            while (true)
            {
                if (bracesCount > 0)
                {
                    if (lexemes[num] == "26")
                    {
                        num++;
                        break;
                    }
                }
                else if (lexemes[num] == "20")
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
                Expression(true);
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

            Operator();

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
                else throw new Exception("Что-то не так с условием(только вот что?)");
            }
        }

        private void For()
        {
            num++;
            if (lexemes[num] == "24") num++;
            else throw new Exception("Ошибкааа");
            fr = true;
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
            fr = false;
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

        private void InOutPut()
        {
            int count = 0;
            num++;
            int i = num;
            if (lexemes[num] == "24") num++;
            else throw new Exception("Ожидался '('");

            while (true)
            {
                if (lexemes[i][0] == '1') throw new Exception("Ошибка в вводе/выводе");
                if (Regex.IsMatch(lexemes[i], @"^(3\d|4\d|25)$") && Regex.IsMatch(lexemes[i + 1], @"^(3\d|4\d|24)$"))
                {
                    lexemes.Insert(i + 1, "222");
                    count++;
                }
                if (Regex.IsMatch(lexemes[i + 1], @"^(20|26|114)$")) break;
                i++;
            }

            for (i = 0; i < count; i++)
            {
                parantheses = 0;
                Expression(true);
            }
            parantheses = 1;
            Expression(true);
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

                if (Regex.IsMatch(lexemes[num], @"^(3\d|4\d)$"))
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

                if (semicolon) { 
                    if ((bracesCount > 0 || fr) && Regex.IsMatch(lexemes[num], @"^(26)$"))
                    {
                        num++;
                        break;
                    }
                    else if (bracesCount <1 && !fr && Regex.IsMatch(lexemes[num], @"^(20||222)$"))
                    {
                        num++;
                        break;
                    }
                    }
                    else if (!semicolon)
                    {
                        if (Regex.IsMatch(lexemes[num], @"^(1\d|20|22|23)$") || (lexemes[num][0] == '3' && lexemes[num + 1] == "27"))
                        {
                            break;
                        }
                    }
                if (Regex.IsMatch(lexemes[num], "^(28|29|210|211|212|213|214|215|216|217|218|219)$")) num++;
                else throw new Exception("Ошибка в выражении(святой дух в помощь)");
            }

            if (parantheses != 0) throw new Exception("Ошибка в скобочках");
        }
    }
}