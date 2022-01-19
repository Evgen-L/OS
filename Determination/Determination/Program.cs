using System;

namespace Determination
{
    class Program
    {
        private const string AUTOMATON_PATH = @"Input\L3.txt";
        static void Main(string[] args)
        {
            StatesReader reader = new StatesReader();
            Automaton simpleAutomaton = new Automaton(reader.ReadStatesFromFile(AUTOMATON_PATH));
            Console.WriteLine($"\nAutomat:\n{simpleAutomaton}");
            Console.WriteLine($"Determinate automat:\n{Determinator.Determine(simpleAutomaton.States)}");
        }
    }
}