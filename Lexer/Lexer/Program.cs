using System;
namespace Lexer
{
    class Program
    {
        private const string PROGRAM_PATH = @"Input\input2.txt";
        static void Main(string[] args)
        {
            TokenReader.ReadTokensFromFile(PROGRAM_PATH)
                .ForEach(token => token.PrintLn());
        }
    }
}