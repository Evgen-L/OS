using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Lexer
{
    class Program
    {
        private static char _currCh;
        private static Queue<char> _expression;
        static void Main(string[] args)
        {
            List<string> tests = new List<string>() {"*", "-a", "-7", 
                "-a*", "-()", "*()",
                "(*)", "(-)", "(a)",
                "(7)", "-77)7", "a-77a7a7*-77a",
                "(-a7", "(7+7)", "((7+7))", "(((7+7)))",
                "(((7+7)))+7", "(((7+7))+7)", "(((7+7)+7))",
                "(7*7)", "((7*7))", "(((7*7)))", "(((7*7)))*7", "(((7*7))*7)", "(((7*7)*7))" 
            };
            
            foreach (string test in tests)
            {
                _expression = new Queue<char>(test);
                GC();
                Console.WriteLine($"{test}: {E() && _expression.Count == 0}");
            }
        }

        private static char GC()
        {
            if (_expression.Count == 0) return _currCh;
            char prevCh = _currCh;
            _currCh = _expression.Dequeue();
            return prevCh;
        }

        private static bool E()
        {
            return T() && A();
        }
        private static bool T()
        {
            return F() && B();
        }
        
        private static bool A()
        {
            if (_currCh == '+')
            {
                GC();
                return T() && A();
            }
            return true;
        }

        private static bool F()
        {
            if (_currCh == '-')
            {
                GC();
                return F();
            }
        
            if (_currCh == '(')
            {
                GC();
                return E() && GC() == ')';
            }

            if (_currCh == '7' || _currCh == 'a')
            {
                GC();
                return true;
            }
            return false;
        }
        private static bool B()
        {
            if (_currCh == '*')
            {
                GC();
                return F() && B();
            }
            return true;
        }
    }
}