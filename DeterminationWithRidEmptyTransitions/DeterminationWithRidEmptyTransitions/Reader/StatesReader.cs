using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Determination
{
    using States = Dictionary<string, StateConnectiongs>;
    public class StatesReader
    {
        public States ReadStatesFromFile(string path)
        {
            StreamReader sr = new StreamReader(path);
            GrammarStatesReader reader = new GrammarStatesReader();
            return reader.ReadGrammarStates(ref sr);
        }
    }
}