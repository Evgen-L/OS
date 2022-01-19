using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace automate
{
    using Moore  = Dictionary<string, List<string>>;
    using ClassAddressStore = Dictionary<string, string>;
    using EquivalentAutomate= Dictionary<string, List<string>>;
    public struct MinimizedMoore
    {
        public string info;
        public string[] states;
        public Moore moore;
    }

    public struct EquivalentMoore 
    {
        public EquivalentAutomate equivalent;
        public Dictionary<string, string> finalStatesSignalsStore;
    }

    public class AutomateMoore : Automate
    {
        const string INCORR_DATA_OF_NUM_STATES_IN_SECOND_LINE       = "incorrect data of the number state in the second line";
        const string INCORR_DATA_OF_NUM_INP_SIGNALS_IN_THIRD_LINE   = "incorrect data of the number input signals in the third line";
        const string INCORR_DATA_OF_NUM_OUT_SIGNALS_IN_FOURTH_LINE  = "incorrect data of the number output signals in the fourth line";
        const string SET_SEALANT = @"\s+";
        const string FINAL_STATE_AND_SIGNAL_SEALANT = "/";
        const short FINAL_STATE_INDEX = 0;
        const short FINAL_OUT_SIGNAL_INDEX = 1;
        const string STATE_SYMBOL = "q";
        const string CLASS_SYMBOL = "C";
        const int FIRST_ELEMENT_LIST = 0;
        
        public AutomateSize automatonSize { get; private set; }
        public string[] states { get; private set; }
        
        public Moore moore { get; private set; }


        public void SetAutomateFromStreamReader(ref StreamReader sr)
        {
            AutomateAssistant assistant = new AutomateAssistant();
            AutomateSize automatonSize = new AutomateSize();
            automatonSize.StatesNum     = assistant.TryShortParseNumSet(sr.ReadLine().Trim(), INCORR_DATA_OF_NUM_STATES_IN_SECOND_LINE);
            automatonSize.InpSignalsNum = assistant.TryShortParseNumSet(sr.ReadLine().Trim(), INCORR_DATA_OF_NUM_INP_SIGNALS_IN_THIRD_LINE);
            automatonSize.OutSignalsNum = assistant.TryShortParseNumSet(sr.ReadLine().Trim(), INCORR_DATA_OF_NUM_OUT_SIGNALS_IN_FOURTH_LINE);

            string[] states = assistant.TrySplitLineFromStreamReader(sr.ReadLine().Trim(), SET_SEALANT, automatonSize.StatesNum);
            Moore moore = DeclareMooreWithStates(ref states);
            string excessPattern = @"^\s*\D*\d*\s*:";
            for (int i = 1; i <= automatonSize.InpSignalsNum; i++)
            {
                string line = Regex.Replace(sr.ReadLine(), excessPattern, "").Trim();
                //Console.WriteLine(line);
                string[] finalStates = assistant.TrySplitLineFromStreamReader(line, SET_SEALANT, automatonSize.StatesNum);
                PushFinalStatesInMoore(ref states, ref finalStates, ref moore);
            }
            if (!sr.EndOfStream) { throw new Exception("the file contains unnecessary data"); }

            this.moore          = moore;
            this.states         = states;
            this.automatonSize  = automatonSize;
        }
        
        private Moore DeclareMooreWithStates(ref string[] states)
        {
            Moore result = new Moore();
            foreach (string state in states)
            {
                result.Add(state, new List<string>());
            }
            return result;
        }
        
        private void PushFinalStatesInMoore(ref string[] states, ref string[] finalStates, ref Moore moore)
        {
            for (int i = 0; i < finalStates.Length; i++)
            {
                moore[states[i]].Add(finalStates[i]);
            }
        }
        
        public void Print()
        {
            Console.WriteLine("Moore:");
            PrintAutomate(moore, automatonSize.InpSignalsNum, states);
        }
        
        private void PrintAutomate(Moore automate, int inpSignalsNum, string[] states) 
        {
            int stateSpaceNum = 2;
            int finalStatesSpaceNum = 5;

            Console.WriteLine($"{string.Join(RepeatText(" ", stateSpaceNum), states)}");

            for (int i = 0; i < inpSignalsNum; i++)
            {
                List<string> finalStates = new List<string>();
                foreach (var oneState in automate)
                    finalStates.Add(oneState.Value[i]);
                Console.WriteLine($" {string.Join(RepeatText(" ", finalStatesSpaceNum), finalStates)}");
            }
            Console.WriteLine();
        }
        public static string RepeatText(string text, int num) 
        {
            return string.Join("", Enumerable.Repeat(text, num));
        }
        public void PrintMinimizedAutomate()
        {

            MinimizedMoore minMoore = GetMinimizedMooreInfo();
            Console.WriteLine(minMoore.info);

            Console.WriteLine("Minimized Moore:");
            PrintAutomate(minMoore.moore, automatonSize.InpSignalsNum, minMoore.states);

        }

        private MinimizedMoore GetMinimizedMooreInfo()
        {
            AutomateAssistant assistant = new AutomateAssistant();
            EquivalentMoore equivalent = GetEquivalentFromMoore();
            ClassesList lastMinimizationClasses = assistant.GetLatestMinimizationClasses(equivalent.equivalent, GetClassesFromMoore());

            MinimizedMoore minimizedMoore = new MinimizedMoore();
            minimizedMoore.moore = GetMinimizedMooreFromClasses(lastMinimizationClasses, equivalent.finalStatesSignalsStore, STATE_SYMBOL);
            minimizedMoore.states = GetStatesFromMoore(minimizedMoore.moore);
            minimizedMoore.info = assistant.GetMinimizedInfoFromClasses(lastMinimizationClasses.Classes, STATE_SYMBOL, CLASS_SYMBOL);
            return minimizedMoore;

        }
        private EquivalentMoore GetEquivalentFromMoore()
        {
            EquivalentMoore result = new EquivalentMoore();
            result.equivalent = new EquivalentAutomate();
            result.finalStatesSignalsStore = new Dictionary<string, string>();
            foreach (var oneState in moore)
            {
                string[] finalStateSignal = oneState.Key.Split(FINAL_STATE_AND_SIGNAL_SEALANT);
                string finalState = finalStateSignal[FINAL_STATE_INDEX];
                string finalSignal = finalStateSignal[FINAL_OUT_SIGNAL_INDEX];
                result.finalStatesSignalsStore.Add(finalState, $"{finalState}/{finalSignal}");
                result.equivalent.Add(finalState, oneState.Value);
            }
            return result;
        }
        
        private ClassesList GetClassesFromMoore()
        {
            ClassesList result = new ClassesList();
            result.Classes = new Dictionary<string, List<string>>();
            result.ReversedСlasses = new Dictionary<string, string>();

            ClassAddressStore addressStore = new ClassAddressStore();

            int index = 1;
            foreach (var state in moore)
            {
                string[] finalStateSignalInfo = state.Key.Split(FINAL_STATE_AND_SIGNAL_SEALANT);
                string key = finalStateSignalInfo[FINAL_OUT_SIGNAL_INDEX];
                string finalState = finalStateSignalInfo[FINAL_STATE_INDEX];
                if (addressStore.ContainsKey(key))
                {
                    result.Classes[addressStore[key]].Add(finalState);
                    result.ReversedСlasses.Add(finalState, addressStore[key]);
                }
                else
                {
                    addressStore.Add(key, index.ToString());
                    result.Classes.Add(index.ToString(), new List<string> { finalState });
                    result.ReversedСlasses.Add(finalState, index.ToString());
                    index++;
                }
            }
            return result;
        }
        
        private Moore GetMinimizedMooreFromClasses(ClassesList classes, Dictionary<string, string> finalStatesSignalsStore, string stateSymbol)
        {
            Moore result = new Moore();
            foreach (var oneClass in classes.Classes)
            {
                string outSignal = finalStatesSignalsStore[oneClass.Value[FIRST_ELEMENT_LIST]].Split(FINAL_STATE_AND_SIGNAL_SEALANT)[FINAL_OUT_SIGNAL_INDEX];
                List<string> finalStates = new List<string>();
                for (int i = 0; i < automatonSize.InpSignalsNum; i++)
                {
                    string finalState =
                        classes.ReversedСlasses[moore[finalStatesSignalsStore[oneClass.Value[FIRST_ELEMENT_LIST]]][i]];
                    finalStates.Add($"{stateSymbol}{finalState}");
                }
                result.Add($"{STATE_SYMBOL}{oneClass.Key}/{outSignal}", finalStates);
            }
            return result;
        }

        private string[] GetStatesFromMoore(Moore moore)
        {
            List<string> result = new List<string>();
            foreach (var state in moore)
            {
                result.Add(state.Key);
            }
            return result.ToArray();
        }
    }
}