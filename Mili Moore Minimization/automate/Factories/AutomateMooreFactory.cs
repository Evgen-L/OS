namespace automate
{
    public class AutomateMooreFactory : AutomateFactory
    {
        public Automate CreateAutomate()
        {
            return new AutomateMoore();
        }
    }
}