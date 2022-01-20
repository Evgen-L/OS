using System;
using System.Collections.Generic;
using System.Linq;

namespace Determination
{
    using States = Dictionary<string, StateConnectiongs>;
    public class Automaton
    {
        public States States { get; }
        public Automaton(Dictionary<string, StateConnectiongs> automaton) => States = automaton;
        

        public override string ToString()
        {
            string result = "";
            foreach (var state in States)
                result += $"{state.Key} -> {state.Value}\n";
            return result;
        }
    }
}