using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Analyzers
{
    class LexicalAnalyz
    {
        private string[] tw = { "int", "floor", "bool", "let", "if", "then", "else", "end_else", "for", "do", "while", "loop", "input", "output", "end" };
        private string[] tl = { ":", ",", "{", "}", "(", ")", ";", "=", "*", "+", "-", " ", "NE", "EQ", "LT", "LE", "GT", "GE", "plus", "min", "or", "mult", "div", "and", "~", "%" };
        private List<string> ti = new List<string>();
        private List<string> tn = new List<string>();
        private string[] lex;
        private List<string> res = new List<string>();

        public LexicalAnalyz(string program)
        {
            bool iscomment = false;
            string[] row = program.Split('\n');
            for (int i = 0; i < row.Length; i++)
            {
                foreach (string lex in row[i].Split(' '))
                {
                    if (lex == "%")
                    {
                        res.Add("225");
                        iscomment = !iscomment;
                    }
                    if (!iscomment)
                    {
                        Lex_Analyzer(lex);
                    }
                }
            }
        }

        private void Lex_Analyzer(string lex)
        {
            if (Regex.IsMatch(lex, @"^[A-Za-z]+\w*$"))
            {
                if (tw.Contains(lex)) res.Add("1" + Array.IndexOf(tw, lex));
                else if (tl.Contains(lex)) res.Add("2" + Array.IndexOf(tl, lex));
                else if (ti.Contains(lex)) res.Add("4" + ti.IndexOf(lex));
                else
                {
                    ti.Add(lex);
                    res.Add("4" + (ti.Count - 1));
                }
            }
            else if (Regex.IsMatch(lex, @"^[0-1]+[Bb]|[0-7]+[Oo]|[0-9]+[Dd]?|[0-9]+[A-F]*[Hh]|[0-9]+.[0-9]+|[0-9]+.[0-9]+[Ee][+-]?[0-9]*" +
                                        @"|[0-9]+[Ee][+-]?[0-9]$"))
            {
                if (tn.Contains(lex)) res.Add("3" + tn.IndexOf(lex));
                else
                {
                    tn.Add(lex);
                    res.Add("3" + (tn.Count - 1));
                }
            }
            else if (tl.Contains(lex)) res.Add("2" + Array.IndexOf(tl, lex));
            else throw new Exception("Данная лексема не содержится в граматике");
        }

        public void Print()
        {
            Console.WriteLine("TW");
            for (int i = 0; i < tw.Length; i++) Console.WriteLine(i + " " + tw[i]);
            Console.WriteLine("\nTL");
            for (int i = 0; i < tl.Length; i++) Console.WriteLine(i + " " + tl[i]);
            Console.WriteLine("\nTI");
            for (int i = 0; i < ti.Count; i++) Console.WriteLine(i + " " + ti[i]);
            Console.WriteLine("\nTN");
            for (int i = 0; i < tn.Count; i++) Console.WriteLine(i + " " + tn[i]);
            Console.WriteLine("\nAnalyze result");
            for (int i = 0; i < res.Count; i++) Console.Write(res[i] + " ");
        }
    }
}
