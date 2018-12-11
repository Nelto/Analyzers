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
            LexicalAnalyz analyzer = new LexicalAnalyz(
                "{\n" +
                "int k = 5\n" +
                "for ( int i = 0 ; i LT 5 ; i + 1 ) k - 1 ;\n" +
                "output ( k ) ;\n" +
                "}"
                );
            analyzer.Print();
            Console.ReadLine();
        }
    }
}
