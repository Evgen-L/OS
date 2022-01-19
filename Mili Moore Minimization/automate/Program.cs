using System;
using System.Collections.Generic;
using System.IO;

namespace automate
{
    class Program
    {
        private const string AUTOMATON_PATH = @"Input\ml2.txt";

        static void Main(string[] args)
        {
            AutomateAssistant assistant = new AutomateAssistant();
            try
            {
                Automate automate = assistant.InitializeAutomateFromFile(AUTOMATON_PATH);
                automate.Print();
                automate.PrintMinimizedAutomate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}