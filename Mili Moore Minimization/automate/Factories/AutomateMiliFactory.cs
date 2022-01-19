using System;
using System.IO;

namespace automate
{
    public class AutomateMiliFactory : AutomateFactory
    {
        public Automate CreateAutomate()
        {
            return new AutomateMili();
        }
    }
}