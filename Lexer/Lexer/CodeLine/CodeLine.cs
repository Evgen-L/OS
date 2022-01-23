namespace Lexer
{
    public struct CodeLine
    {
        public int Index;
        public string Text;

        public CodeLine(int index, string text)
        {
            Index = index;
            Text = text;
        }
    }
}