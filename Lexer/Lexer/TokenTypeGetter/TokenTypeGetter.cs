namespace Lexer
{
    public static class TokenTypeGetter
    {
        public static TokenType GetTokenTypeByName(string name)
        {
            if (TokenStorage.Storage.ContainsKey(name))         
                return TokenStorage.Storage[name];
            
            return TokenType.VARIABLE;
        }
    }
}