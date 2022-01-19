using System.Collections.Generic;
using System.IO;

namespace Determination
{
    using States = Dictionary<string, StateConnectiongs>;
    public abstract class GrammarStatesReader
    {
        protected const char EMPTY_SYMBOL = 'H';
        protected const int FIRST_SYMBOL_INDEX = 0;
        
        protected const int RSIGNAL_INDEX = 0;
        protected const int RSTATE_INDEX = 1;
        
        protected const int LSIGNAL_INDEX = 1;
        protected const int LSTATE_INDEX = 0;
        
        protected const string SEPARATOR_PATTERN = @"\s*[:]\s*|\s*[|]\s*";

        public abstract States ReadGrammarStates(ref StreamReader sr);
    }
}