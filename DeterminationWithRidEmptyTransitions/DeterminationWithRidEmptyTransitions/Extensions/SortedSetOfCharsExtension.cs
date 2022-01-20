using System.Collections.Generic;
using System.Linq;

namespace DeterminationWithRidEmptyTransitions.Extensions
{
    public static class SortedSetOfCharsExtension
    {
        public static string ToText(this SortedSet<char> set) => new string(set.ToArray());
    }
}