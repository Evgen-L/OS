using System;
using System.Collections.Generic;
using System.Linq;

namespace LL1
{
    class Program
    {
        private static char _currCh;
        private static Queue<char> _expression;
        private static TableRules _table;
        private const char END_SYMBOL = '\0';
        private static int NULL = -1;
        private static Stack<Rule> _reserveRules = new Stack<Rule>();

        static void Main(string[] args)
        {
            _table = new TableRules();
            PrepareTable(ref _table);
            // List<string> tests = new List<string>()
            // {
            //     "-7-7", "*", "-a", "-7", "-a*", "-()", "*()", "(*)", "(-)", "(a)", "(7)", "-77)7", "a-77a7a7*-77a",
            //     "(-a7", "(7+7)", "((7+7))", "(((7+7)))",
            //     "(((7+7)))+7", "(((7+7))+7)", "(((7+7)+7))",
            //     "(7*7)", "((7*7))", "(((7*7)))", "(((7*7)))*7", "(((7*7))*7)", "(((7*7)*7))"
            // };
            
            List<string> tests = new List<string>()
            {
                "(-7)"
            };

            foreach (string test in tests)
            {
                _expression = new Queue<char>(test);
                GC();
                Console.WriteLine($"{test}: {ValidExpression()}");
            }
        }

        private static bool ValidExpression()
        {
            Rule currRule = _table.Rules.First();
            Rule newRule;

            for (;;)
            {
                if (currRule.GuideSymbols.Contains(_currCh))
                { 
                    if (currRule.End) return true;
                    newRule = TakeRuleByIndex(currRule.Pointer);
                }
                
                else if (currRule.Error) return false;
                else newRule = TakeRuleByIndex(currRule.IdRule + 1);

                if (currRule.Shift) GC();

                if (currRule.InStack) _reserveRules.Push(_table.Rules[currRule.IdRule + 1]);
                
                currRule = newRule;
            }
        }
        
        private static Rule TakeRuleByIndex(int index)
        {
            return index == NULL ? _reserveRules.Pop() : _table.Rules[index];
        }

        private static void GC() => _currCh = _expression.Count == 0 ? END_SYMBOL : _expression.Dequeue();

        private static void PrepareTable(ref TableRules table)
        {
            table.AddRule(new Rule(0, "7a-(", false, true, 1, false, false));
            table.AddRule(new Rule(1, "7a-(", false, true, 3, true, false));
            table.AddRule(new Rule(2, "\0", true, true, NULL, false, true));
            table.AddRule(new Rule(3, "7a-(", false, true, 4, false, false));
            table.AddRule(new Rule(4, "7a-(", false, true, 14, true, false));
            table.AddRule(new Rule(5, "+)\0", false, true, 6, false, false));
            table.AddRule(new Rule(6, "+", false, false, 11, false, false));
            table.AddRule(new Rule(7, ")", false, false, 10, false, false));
            table.AddRule(new Rule(8, "\0", false, true, 9, false, false));
            table.AddRule(new Rule(9, "\0", false, true, NULL, false, false));
            table.AddRule(new Rule(10, ")", false, true, NULL, false, false));
            table.AddRule(new Rule(11, "+", true, true, 12, false, false));
            table.AddRule(new Rule(12, "7a-(", false, true, 14, true, false));
            table.AddRule(new Rule(13, "+)\0", false, true, 6, false, false));
            table.AddRule(new Rule(14, "7a-(", false, true, 15, false, false));
            table.AddRule(new Rule(15, "7a-(", false, true, 27, true, false));
            table.AddRule(new Rule(16, "*+)\0", false, true, 17, false, false));
            table.AddRule(new Rule(17, "*", false, false, 24, false, false));
            table.AddRule(new Rule(18, "+", false, false, 23, false, false));
            table.AddRule(new Rule(19, ")", false, false, 22, false, false));
            table.AddRule(new Rule(20, "\0", false, true, 21, false, false));
            table.AddRule(new Rule(21, "\0", false, true, NULL, false, false));
            table.AddRule(new Rule(22, ")", false, true, NULL, false, false));
            table.AddRule(new Rule(23, "+", false, true, NULL, false, false));
            table.AddRule(new Rule(24, "*", true, true, 25, false, false));
            table.AddRule(new Rule(25, "7a-(", false, true, 27, true, false));
            table.AddRule(new Rule(26, "*+)\0", false, true, 17, false, false));
            table.AddRule(new Rule(27, "7", false, false, 31, false, false));
            table.AddRule(new Rule(28, "a", false, false, 32, false, false));
            table.AddRule(new Rule(29, "-", false, false, 33, false, false));
            table.AddRule(new Rule(30, "(", false, true, 35, false, false));
            table.AddRule(new Rule(31, "7", true, true, NULL, false, false));
            table.AddRule(new Rule(32, "a", true, true, NULL, false, false));
            table.AddRule(new Rule(33, "-", true, true, 34, false, false));
            table.AddRule(new Rule(34, "7a-(", false, true, 27, false, false));
            table.AddRule(new Rule(35, "(", true, true, 36, false, false));
            table.AddRule(new Rule(36, "7a-(", false, true, 3, true, false));
            table.AddRule(new Rule(37, ")", true, true, NULL, false, false));
        }
    }
}

// Совпало                - двигаем по указателю
// Сдвиг                  - сдвигаем выражение,
// Стек                   - заносим в стек указатель на следующую строку(от текущей)
// Совпало И pointer NULL - берем из стека указатель
// Не совпало и Error     - return false
// Не совпало и не Error  - переходим на следующее правило
// Если конец сбора       - return true

//ошибки по умолчанию стоят при разборе нетерминалов, у которых нет терминалов, если разбираем нетерминал у которого есть терминалы, то ошибка будет стоять у последнего символа
//сдвиг там где раазбор символов
//стек ставим если мы должны вернуться  