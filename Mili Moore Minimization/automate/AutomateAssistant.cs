using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace automate
{
    using Mili  = Dictionary<string, List<FinalStateMilli>>;
    using EquivalentAutomaton = Dictionary<string, List<string>>;
    using ClassAddressStore   = Dictionary<string, string>;
    public enum AutomatonType
    {
        MILI,
        MOORE,
        NO_AUTOMAT
    }
    
    public class AutomateAssistant
    {
        const string INCORRECT_AUTOMAT_FORMAT = "incorrect automat name";
        const int FIRST_ELEMENT_LIST = 0;

        static readonly Dictionary<string, AutomatonType> AutomatonTypeStorage = new()
        {
            { "ML", AutomatonType.MILI },
            { "MR", AutomatonType.MOORE },
        };
        public Automate InitializeAutomateFromFile(string path)
        {
            StreamReader sr = new StreamReader(path);
            switch (GetTypeAutomaton(sr.ReadLine().Trim()))
            {
                case AutomatonType.MILI:
                    return InitializeAutomateFromSTreamReader(new AutomateMiliFactory(), ref sr);

                case AutomatonType.MOORE:
                    return InitializeAutomateFromSTreamReader(new AutomateMooreFactory(), ref sr);

                default:
                    throw new Exception($"{INCORRECT_AUTOMAT_FORMAT} in first line");
            }
        }
        private static Automate InitializeAutomateFromSTreamReader(AutomateFactory af, ref StreamReader sr)
        {
            Automate automate = af.CreateAutomate();
            automate.SetAutomateFromStreamReader(ref sr);
            sr.Close();
            return automate;
        }
        private static AutomatonType GetTypeAutomaton(string key)
        {
            return AutomatonTypeStorage.ContainsKey(key.Trim().ToUpper())
                ? AutomatonTypeStorage[key.Trim().ToUpper()]
                : AutomatonType.NO_AUTOMAT;
        }
        public short TryShortParseNumSet(string text, string errorMessage)
        {
            short result;
            if (!short.TryParse(text, out result) || result <= 0) throw new Exception(errorMessage);
            return result;
        }
        public string[] TrySplitLineFromStreamReader(string line, string sealant, short expectedNumElement)
        {
            string[] result = Regex.Split(line, sealant);
            if (result.Length != expectedNumElement)
                throw new Exception("incorrect data of number elements");
            return result;
        }
        public ClassesList GetLatestMinimizationClasses(EquivalentAutomaton equival, ClassesList firstClasses)
        {
            ClassesList currClasses                 = new ClassesList();
                        currClasses.Classes         = new Dictionary<string, List<string>>();
                        currClasses.ReversedСlasses = new Dictionary<string, string>();
                        
            ClassesList nextClasses = firstClasses;
            while (currClasses.Classes.Count != nextClasses.Classes.Count)
            {
                currClasses = nextClasses;
                nextClasses = GetNextClassesFromEquivalent(ref equival, ref currClasses);
            }
            return nextClasses;
        }
        private static ClassesList GetNextClassesFromEquivalent(ref EquivalentAutomaton equival, ref ClassesList currClasses)
        {
            ClassesList result     = new ClassesList();
            result.Classes         = new Dictionary<string, List<string>>();
            result.ReversedСlasses = new Dictionary<string, string>();
                        
            ClassAddressStore addressStore = new ClassAddressStore();

            int index = 1;
            foreach (var oneClass in currClasses.Classes)
            {
                foreach (string state in oneClass.Value)
                {
                    string key = $"{oneClass.Key}";
                    foreach (var finalState in equival[state])
                        key += currClasses.ReversedСlasses[finalState];
                    
                    if (addressStore.ContainsKey(key))
                    {
                        result.Classes[addressStore[key]].Add(state);
                        result.ReversedСlasses.Add(state, addressStore[key]);
                    }
                    else
                    {    
                        addressStore.Add(key, index.ToString());
                        result.Classes.Add(index.ToString(), new List<string>{state});
                        result.ReversedСlasses.Add(state, index.ToString());
                        index++;
                    }
                }
            }
            return result;
        }
        public string GetMinimizedInfoFromClasses(Dictionary<string, List<string>> classes, string stateSymbol, string classSymbol) 
        {
            string result = "MinimizedInfo:\n";
            foreach (var oneClass in classes) 
            {
                result += $"Class {classSymbol}{oneClass.Key}: {string.Join(" ", oneClass.Value)}\n" +
                    $"State: {oneClass.Value[FIRST_ELEMENT_LIST]}\n" +
                    $"Designation: {stateSymbol}{oneClass.Key}\n\n";
            }
            return result;
        }
    }
}