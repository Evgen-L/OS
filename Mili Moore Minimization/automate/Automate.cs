using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace automate
{
    using Mili  = Dictionary<string, List<FinalStateMilli>>;
    using Moore = Dictionary<string[], Dictionary<string, string>>;
    
    public struct FinalStateMilli
    {
        public string state;
        public string outSignal;

        public FinalStateMilli(string state, string outSignal)
        {
            this.state = state;
            this.outSignal = outSignal;
        }
    }
    
    
    



    public struct AutomateSize
    {
        public short StatesNum;
        public short InpSignalsNum;
        public short OutSignalsNum;

        public AutomateSize(short statesNum, short inpSignalsNum, short outSignalsNum) 
        {
            this.StatesNum = statesNum;
            this.InpSignalsNum = inpSignalsNum;
            this.OutSignalsNum = outSignalsNum;
        }
    }
    public interface Automate
    {
        public void SetAutomateFromStreamReader(ref StreamReader sr)
        {
        }

        public void Print()
        {
            
        }
        
        public void PrintMinimizedAutomate()
        {
            
        }
        
    }
}