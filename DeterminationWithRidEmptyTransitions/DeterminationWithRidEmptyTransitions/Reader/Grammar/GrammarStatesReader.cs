using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Determination
{
    using States = Dictionary<string, StateConnectiongs>;
    public class GrammarStatesReader
    {
        protected const char EMPTY_SYMBOL = 'H';
        protected const int FIRST_SYMBOL_INDEX = 0;
        
        protected const int RSIGNAL_INDEX = 0;
        protected const int RSTATE_INDEX = 1;

        protected const string SEPARATOR_PATTERN = @"\s*[:]\s*|\s*[|]\s*";

        public Dictionary<string, StateConnectiongs> ReadGrammarStates(ref StreamReader sr)
        {
            States result = new Dictionary<string, StateConnectiongs>();
            while (!sr.EndOfStream)
            {
                List<string> stateLine = new List<string>(Regex.Split(sr.ReadLine(), SEPARATOR_PATTERN));
                result.Add(stateLine.First(), GetRGrammarConnectiongs(stateLine.Skip(1).ToList()));
            }
            return result;
        }
        
        private StateConnectiongs GetRGrammarConnectiongs(List<string> textConnects)
        {
            StateConnectiongs result = new StateConnectiongs();
            textConnects.ForEach(con =>
            {
                if (con.Length == 1) result.AddConnectiong(con[FIRST_SYMBOL_INDEX].ToString(), EMPTY_SYMBOL);
                if (con.Length == 2) result.AddConnectiong(con[RSIGNAL_INDEX].ToString(), con[RSTATE_INDEX]);
            });
            
            return result;
        }
    }
}