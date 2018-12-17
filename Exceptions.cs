using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzers
{
    class Exceptions
    {
        static public string GetExcep(int id)
        {
            switch (id)
            {
                case 0: return "Не закрыт комментарий";
                case 1: return "Данная лексема не содержится в граматике";

                case 2: return "Ожидался end";
                case 3: return "Ошибка в фигурных скобках";
                case 4: return "Неизвестный оператор";
                case 5: return "Ошибка в описаниее данных";
                case 6: return "Ошибка в операторе присваивания";
                case 7: return "Ошибка в условном операторе";
                case 8: return "Ошибка в операторе for";
                case 9: return "Не найден конец цикла while";
                case 10: return "Ошибка в вводе/выводе";
                case 11: return "Ошибка в выражении";

                case 12: return "Повторное обьявление переменной";
                case 13: return "Используется переменная, но она нигде не обьявлена";
                case 14: return "Ожидался тип bool";
                case 15: return "Операции or, and и ~ не применимы к типам int и float";
                case 16: return "Операция не применима к типу bool";
                case 17: return "Ошибка при использовании операции сравнения";
                case 18: return "Неверный тип данных";
                case 19: return "Ожидалось выражение";
                case 20: return "Ожидалось выражение тип int или float";
                default: return "";
            }
        }
    }
}
