using System;
using System.Collections.Generic;
using System.Linq;
using DeterminationWithRidEmptyTransitions.Extensions;

namespace Determination
{
    using States = Dictionary<string, StateConnectiongs>;
    using State = KeyValuePair<string, StateConnectiongs>;

    public static class RipEmptyTransitionsDeterminator
    {
        private const string EMPTY_TRANSITION_SYMBOL = "E"; 
        //public
        public static Automaton Determine(States equivalent)
        {
            States resultStates = new States();
            Stack<string> childStates = new Stack<string>();
            childStates.Push(equivalent.First().Key);
            while (childStates.Count != 0)
            {
                State currState = GetStateFromEquivalent(childStates.Pop(), equivalent);
                
                if (!resultStates.ContainsKey(currState.Key))
                    resultStates.Add(currState.Key, currState.Value);
                
                PushNewStates(currState.Value, ref childStates, resultStates);
            }
            
            return new Automaton(resultStates);
        }
        
        //private
        private static State GetStateFromEquivalent(string combinedKey, States equivalent)
        {
            State result = new State(combinedKey, new StateConnectiongs());
            combinedKey += GetStatesByEmptyTransitions(combinedKey, ref equivalent);
            combinedKey.ToCharArray().ToList().ForEach(ch =>
            {
                if (equivalent.ContainsKey(ch.ToString()))
                    foreach (var connect in equivalent[ch.ToString()].Connectiongs
                        .Where(con => con.Key != EMPTY_TRANSITION_SYMBOL))
                        result.Value.AddConnectiong(connect.Key, connect.Value);
            });
            
            return result;
        }

        private static string GetStatesByEmptyTransitions(string combinedKey, ref States equivalent)
        {
            string result = "";
            foreach (char ch in combinedKey.ToCharArray().ToList())
                if (equivalent.ContainsKey(ch.ToString()))
                    foreach (var connect in equivalent[ch.ToString()].Connectiongs
                        .Where(con => con.Key == EMPTY_TRANSITION_SYMBOL))
                        result += connect.Value.ToText() + GetStatesByEmptyTransitions(connect.Value.ToText(), ref equivalent);
            
            return result;
        }
        private static void PushNewStates(StateConnectiongs connects, ref Stack<string> to, States resultStates)
        {
            foreach (var connect in connects.Connectiongs)
                if (!resultStates.ContainsKey(connect.Value.ToText())) 
                    to.Push(connect.Value.ToText());
        }
    }
}