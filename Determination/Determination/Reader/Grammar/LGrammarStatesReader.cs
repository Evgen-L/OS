using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Determination
{
    using States = Dictionary<string, StateConnectiongs>;
    public class LGrammarStatesReader : GrammarStatesReader
    {
        public override Dictionary<string, StateConnectiongs> ReadGrammarStates(ref StreamReader sr)
        {
            States result = new Dictionary<string, StateConnectiongs>();
            while (!sr.EndOfStream)
            {
                List<string> stateLine = new List<string>(Regex.Split(sr.ReadLine(), SEPARATOR_PATTERN));
                AddLStatesFromLine(ref stateLine, ref result);
            }
            return result;
        }
        
        private void AddLStatesFromLine(ref List<string> stateLine, ref States states)
        {
            foreach (var con in stateLine.Skip(1).ToList())
            {
                if (con.Length == 1)
                {
                    if (!states.ContainsKey(EMPTY_SYMBOL.ToString())) 
                        states = states.AddAsFirst(EMPTY_SYMBOL.ToString(), new StateConnectiongs());
                        
                    states[EMPTY_SYMBOL.ToString()].AddConnectiong(con[FIRST_SYMBOL_INDEX].ToString(), stateLine.First()[FIRST_SYMBOL_INDEX]);
                }
                    
                if (con.Length == 2)
                {
                    if (!states.ContainsKey(con[LSTATE_INDEX].ToString())) 
                        states.Add(con[LSTATE_INDEX].ToString(), new StateConnectiongs());
                        
                    states[con[LSTATE_INDEX].ToString()].AddConnectiong(con[LSIGNAL_INDEX].ToString(), stateLine.First()[FIRST_SYMBOL_INDEX]);
                } 
            }
        }
    }
}