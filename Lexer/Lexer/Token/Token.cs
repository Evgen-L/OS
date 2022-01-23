using System;

namespace Lexer
{
    public class Token
    {
        public string Name     { get; }
        public TokenType Type  { get; }
        public int Position    { get; }
        public int Line        { get; }

        public Token(string name, TokenType type, int position, int line)
        {
            Name = name;
            Type = type;
            Position = position;
            Line = line;
        }

        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan; 
            Console.Write("token: ");
            Console.ResetColor();
            Console.Write($"\"{Name}\" ");
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("type: ");
            Console.ResetColor();
            Console.Write($"{Type} ");
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("line: ");
            Console.ResetColor();
            Console.Write($"{Line} ");
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("position: ");
            Console.ResetColor();
            Console.Write($"{Position} ");
        }

        public void PrintLn()
        {
            Print();
            Console.WriteLine();
        }
    }
}