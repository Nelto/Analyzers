using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzers
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                LexicalAnalyz analyzer = new LexicalAnalyz(
                    "(*это тест*)\n" +
                    "int k:\n" +
                    "int s:\n" +
                    "int a:\n" +
                    "k = 5: a = 0: s = 1:\n" +
                    "if k LT 1 then {\n" +
                    "do while k NE s\n" +
                    "a = a plus 2;\n" +
                    "loop\n" +
                    "(*и это тест*)\n" +
                    "}\n" +
                    "end_else " +
                    "for ( i ; i LT 5 ; i plus 1 ) a = k min 1:\n" +
                    "output ( k s plus 5 (a div k)) :\n" +
                    "end" +
                    "(*это последний тест*)"
                    );
                analyzer.Print();
                SyntacAnalyz syntac = new SyntacAnalyz(analyzer.GetRes()); 
            }
            catch(Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}
