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
            GrammarStatesReader reader = GetReaderByType(GrammarTypeGetter.GetGrammarType(sr?.ReadLine()?.Trim()));
            return reader.ReadGrammarStates(ref sr);
        }

        private GrammarStatesReader GetReaderByType(GrammarType type)
        {
            switch (type)
            {
                case GrammarType.LEFT:
                    return new LGrammarStatesReader();

                case GrammarType.RIGHT:
                    return new RGrammarStatesReader();

                default:
                    throw new Exception($"Incorrect grammar format in first line");
            }
        }
    }
}