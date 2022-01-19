using System.Collections.Generic;

namespace Determination
{
    public static class GrammarTypeGetter
    {
        static readonly Dictionary<string, GrammarType> GrammarTypeStorage = new()
        {
            { "LEFT",  GrammarType.LEFT  },
            { "L",     GrammarType.LEFT  },
            { "RIGHT", GrammarType.RIGHT },
            { "R",     GrammarType.RIGHT }
        };
        public static GrammarType GetGrammarType(string text)
        {
            return GrammarTypeStorage.ContainsKey(text.Trim().ToUpper())
                ? GrammarTypeStorage[text.Trim().ToUpper()]
                : GrammarType.NO_GRAMMAR;
        }
    }
}