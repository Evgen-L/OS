using System;
using System.Collections.Generic;
using System.Linq;
using DeterminationWithRidEmptyTransitions.Extensions;
using Microsoft.VisualBasic;

namespace Determination
{
    public class StateConnectiongs
    {
        //signal -> state
        public SortedDictionary<string, SortedSet<char>> Connectiongs { get; private set; } = new();

        public StateConnectiongs() => Connectiongs = new (){};
        
        //public
        public void AddConnectiong(string signal, char state)
        {
            if (Connectiongs.ContainsKey(signal))
                Connectiongs[signal].Add(state);
            else
                Connectiongs.Add(signal, new SortedSet<char>(){state});
        }
        
        public void AddConnectiong(string signal, SortedSet<char> states)
        {
            if (Connectiongs.ContainsKey(signal))
                foreach (var ch in states)
                    Connectiongs[signal].Add(ch);
            else
                Connectiongs.Add(signal, new SortedSet<char>(states));
        }
        
        public override string ToString()
        {
            string result = ToStringFirstConnect();
            foreach (var connect in Connectiongs.Skip(1)) 
                result += $" | {connect.Key}{connect.Value.ToText()}";
            return result;
        }
        
        //private
        private string ToStringFirstConnect() => 
            $"{Connectiongs.FirstOrDefault().Key}{(Connectiongs.FirstOrDefault().Value == null ? "" : Connectiongs.FirstOrDefault().Value.ToText())}";
    }
}