using System.Collections.Generic;

namespace LL1
{
    public class TableRules
    {
        public List<Rule> Rules { get; } = new List<Rule>();

        public void AddRule(Rule rule)
        {
            Rules.Add(rule);
        }
    }
}