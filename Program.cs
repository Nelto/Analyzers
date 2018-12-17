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
                    "bool a:" +
                    "a = true:\n"+
                    "int i:"+
                    "float t:\n"+
                    "t = 52e+2:\n"+
                    "int s:\n" +
                    "input ( s ):\n"+
                    "s = 1001B:\n" +
                    "if a or t NE s and t EQ 1e5 then {\n" +
                    "do while t NE s\n" +
                    "t = t plus 2;\n" +
                    "loop\n" +
                    "(*и это тест*)\n" +
                    "}\n" +
                    "end_else " +
                    "for ( i ; i LT 5 ; i plus 1 ) s = s min 1:\n" +
                    "output ( s plus 5 (t div s)) :\n" +
                    "end" +
                    "(*это последний тест*)"
                    );
                analyzer.Print();
                SyntacAnalyz syntac = new SyntacAnalyz(analyzer.GetRes());
                SemanticAnalyz semantic = new SemanticAnalyz(analyzer.GetTN(), syntac.GetLexemes());
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
