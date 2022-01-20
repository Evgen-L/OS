using System;
using Determination;

namespace DeterminationWithRidEmptyTransitions
{
    class Program
    {
        private const string AUTOMATON_PATH = @"Input\1.txt";
        static void Main(string[] args)
        {
            StatesReader reader = new StatesReader();
            Automaton simpleAutomaton = new Automaton(reader.ReadStatesFromFile(AUTOMATON_PATH));
            Console.WriteLine($"\nAutomat:\n{simpleAutomaton}");
            Console.WriteLine($"Determinate automat:\n{RipEmptyTransitionsDeterminator.Determine(simpleAutomaton.States)}");
        }
    }
}