using System;
using Antlr4.Runtime;

namespace ANTLRTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test ANTLR4.");

            AntlrFileStream input = new AntlrFileStream("input.bib");
            Lexer lexer = new LexerInterpreter();

        }
    }
}
