namespace Lexer
{
    public static class TokenBuilder
    {
        public static Token Construct(string name, int numberLine, int position)
            => new Token(name, TokenTypeGetter.GetTokenTypeByName(name), position, numberLine);
        
        public static Token Construct(string name, TokenType type, int numberLine, int position)
            => new Token(name, type, position, numberLine);
    }
}