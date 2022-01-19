using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace automate
{
    using Mili  = Dictionary<string, List<FinalStateMilli>>;
    using ClassAddressStore   = Dictionary<string, string>;
    using EquivalentAutomaton = Dictionary<string, List<string>>;


    public struct ClassesList
    {
        public Dictionary<string, List<string>> Classes;
        public Dictionary<string, string> ReversedСlasses;
    }

    public struct MinimizedMili 
    {
        public string Info;
        public string[] States;
        public Mili Mili;
    }


    public class AutomateMili : Automate
    {
        const string INCORR_DATA_OF_NUM_STATES_IN_SECOND_LINE       = "incorrect data of the number state in the second line";
        const string INCORR_DATA_OF_NUM_INP_SIGNALS_IN_THIRD_LINE   = "incorrect data of the number input signals in the third line";
        const string INCORR_DATA_OF_NUM_OUT_SIGNALS_IN_FOURTH_LINE  = "incorrect data of the number output signals in the fourth line";
        const string SET_SEALANT = @"\s+";
        const string FINAL_STATE_AND_SIGNAL_SEALANT = "/";
        const string STATE_SYMBOL = "S";
        const string CLASS_SYMBOL = "C";
        const short FINAL_STATE_INDEX = 0;
        const short FINAL_OUT_SIGNAL_INDEX = 1;
        const int FIRST_ELEMENT_LIST = 0;



        public AutomateSize AutomatonSize { get; private set; }
        public string[] States { get; private set; }
        public Mili Mili { get; private set; }
        
        public void SetAutomateFromStreamReader(ref StreamReader sr)
        {
            AutomateAssistant assistant = new AutomateAssistant();
            AutomateSize automatonSize = new AutomateSize();
            automatonSize.StatesNum     = assistant.TryShortParseNumSet(sr.ReadLine().Trim(), INCORR_DATA_OF_NUM_STATES_IN_SECOND_LINE);
            automatonSize.InpSignalsNum = assistant.TryShortParseNumSet(sr.ReadLine().Trim(), INCORR_DATA_OF_NUM_INP_SIGNALS_IN_THIRD_LINE);
            automatonSize.OutSignalsNum = assistant.TryShortParseNumSet(sr.ReadLine().Trim(), INCORR_DATA_OF_NUM_OUT_SIGNALS_IN_FOURTH_LINE);

            string[] states = assistant.TrySplitLineFromStreamReader(sr.ReadLine().Trim(), SET_SEALANT, automatonSize.StatesNum);
            Mili mili = DeclareMiliWithStates(ref states);
            string excessPattern = @"^\s*\D*\d*\s*:";
            for (int i = 1; i <= automatonSize.InpSignalsNum; i++)
            {
                string line = Regex.Replace(sr.ReadLine(), excessPattern, "").Trim();
                string[] finalStates = assistant.TrySplitLineFromStreamReader(line, SET_SEALANT, automatonSize.StatesNum);
                PushFinalStatesInMili(ref states, ref finalStates, ref mili);
            }
            if (!sr.EndOfStream) { throw new Exception("the file contains unnecessary data"); }

            this.Mili          = mili;
            this.States        = states;
            this.AutomatonSize = automatonSize;
        }
        private Mili DeclareMiliWithStates(ref string[] states)
        {
            Mili result = new Mili();
            foreach (string state in states)
            {
                result.Add(state, new List<FinalStateMilli>());
            }
            return result;
        }
        private void PushFinalStatesInMili(ref string[] states, ref string[] finalStates, ref Mili mili)
        {
            for (int i = 0; i < finalStates.Length; i++)
            {
                string[] finalState = finalStates[i].Split(FINAL_STATE_AND_SIGNAL_SEALANT);
                mili[states[i]].Add(new FinalStateMilli(finalState[FINAL_STATE_INDEX], finalState[FINAL_OUT_SIGNAL_INDEX]));
            }
        }
        public void Print()
        {
            Console.WriteLine("Mili:");
            PrintAutomate(Mili, AutomatonSize.InpSignalsNum, States);
        }
        private void PrintAutomate(Mili automate, int inpSignalsNum, string[] states) 
        {
            int stateSpaceNum = 5;
            int finalStatesSpaceNum = 2;

            Console.WriteLine($" {string.Join(RepeatText(" ", stateSpaceNum), states)}");

            for (int i = 0; i < inpSignalsNum; i++)
            {
                List<string> finalStatesSignals = new List<string>();
                foreach (var oneState in automate)
                    finalStatesSignals.Add(GetFinalStateText(oneState.Value[i]));
                Console.WriteLine(string.Join(RepeatText(" ", finalStatesSpaceNum), finalStatesSignals));
            }
            Console.WriteLine();
        }
        private static string GetFinalStateText(FinalStateMilli fsm)
        {
            return $"{fsm.state}/{fsm.outSignal}";
        }
        private static string RepeatText(string text, int num) 
        {
            return string.Join("", Enumerable.Repeat(text, num));
        }
        public void PrintMinimizedAutomate()
        {
            MinimizedMili minMili = GetMinimizedMiliInfo();
            Console.WriteLine(minMili.Info);

            Console.WriteLine("Minimized Mili:");
            PrintAutomate(minMili.Mili, AutomatonSize.InpSignalsNum, minMili.States);
        }
        private EquivalentAutomaton GetEquivalentFromMili()
        {
            EquivalentAutomaton result = new EquivalentAutomaton();
            foreach (var oneState in Mili)
            {
                List<string> states = GetFinalStatesWithoutSignalsFromList(oneState.Value);
                result.Add(oneState.Key, states);
            }
            return result;
        }
        private static List<string> GetFinalStatesWithoutSignalsFromList(List<FinalStateMilli> finalStates)
        {
            return finalStates.Select(x => x.state).ToList();
        }

        private ClassesList GetClassesFromMili()
        {
            ClassesList result                 = new ClassesList();
                        result.Classes         = new Dictionary<string, List<string>>();
                        result.ReversedСlasses = new Dictionary<string, string>();
                        
            ClassAddressStore addressStore = new ClassAddressStore();            
            
            int index = 1;
            foreach (var state in Mili)
            {
                string key = "";
                foreach (var finalState in state.Value) 
                    key += finalState.outSignal;
                
                
                if (addressStore.ContainsKey(key))
                {
                    result.Classes[addressStore[key]].Add(state.Key);
                    result.ReversedСlasses.Add(state.Key, addressStore[key]);
                }
                else
                {
                    addressStore.Add(key, index.ToString());
                    result.Classes.Add(index.ToString(), new List<string>{state.Key});
                    result.ReversedСlasses.Add(state.Key, index.ToString());
                    index++;
                }
            }
            return result;
        }
        private MinimizedMili GetMinimizedMiliInfo() 
        {
            AutomateAssistant assistant = new AutomateAssistant();
            ClassesList lastMinimizationClasses = assistant.GetLatestMinimizationClasses(GetEquivalentFromMili(), GetClassesFromMili());
            
            MinimizedMili minimizedMili = new MinimizedMili();           
            minimizedMili.Mili = GetMinimizedMiliFromClasses(lastMinimizationClasses, STATE_SYMBOL);
            minimizedMili.States = GetStatesFromMili(minimizedMili.Mili);
            minimizedMili.Info = assistant.GetMinimizedInfoFromClasses(lastMinimizationClasses.Classes, STATE_SYMBOL, CLASS_SYMBOL);
            return minimizedMili;

        }
        private Mili GetMinimizedMiliFromClasses(ClassesList classes, string stateSymbol) 
        {
            Mili result = new Mili();
            foreach (var oneClass in classes.Classes)
            {
                List<FinalStateMilli> finalStates = new List<FinalStateMilli>();
                for (int i = 0; i < AutomatonSize.InpSignalsNum; i++) 
                {
                    string finalState = GetClassByStateMiliFromReversedClasses(Mili[oneClass.Value[FIRST_ELEMENT_LIST]][i].state, classes.ReversedСlasses);
                    string finalOutSignal = Mili[oneClass.Value[FIRST_ELEMENT_LIST]][i].outSignal;
                    finalStates.Add(new FinalStateMilli($"{stateSymbol}{finalState}", finalOutSignal));
                }
                result.Add($"{STATE_SYMBOL}{oneClass.Key}", finalStates);
            }
            return result;
        }
        private string GetClassByStateMiliFromReversedClasses(string state, Dictionary<string, string> classes) 
        {
            return classes[state];
        }
        private string[] GetStatesFromMili(Mili mili) 
        {
            List<string> result = new List<string>();
            foreach (var state in mili) 
            {
                result.Add(state.Key);
            }
            return result.ToArray();
        }
    }
}