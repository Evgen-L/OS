using System.Collections.Generic;

namespace Lexer
{
    public static class TokenStorage
    {
        public static readonly Dictionary<string, TokenType> Storage = new ()
        {
            //ComplexTokens
            { "PROGRAM",   TokenType.RESERVED_WORD },
            { "PROCEDURE", TokenType.RESERVED_WORD },
            { "BEGIN",     TokenType.RESERVED_WORD },
            { "VAR",       TokenType.RESERVED_WORD },
            { "STRING",    TokenType.RESERVED_WORD },
            { "INTEGER",   TokenType.RESERVED_WORD },
            { "BOOLEAN",   TokenType.RESERVED_WORD },
            { "TEXT",      TokenType.RESERVED_WORD },
            { "CHAR",      TokenType.RESERVED_WORD },
            { "END",       TokenType.RESERVED_WORD },
            { "IF",        TokenType.RESERVED_WORD },
            { "THEN",      TokenType.RESERVED_WORD },
            { "ELSE",      TokenType.RESERVED_WORD },
            { "TRUE",      TokenType.RESERVED_WORD },
            { "FALSE",     TokenType.RESERVED_WORD },
            { "RESET",     TokenType.RESERVED_WORD },
            { "ASSIGN",    TokenType.RESERVED_WORD },
            { "REWRITE",   TokenType.RESERVED_WORD },
            { "CLOSE",     TokenType.RESERVED_WORD },
            { "WHILE",     TokenType.RESERVED_WORD },
            { "FOR",       TokenType.RESERVED_WORD },
            { "DO",        TokenType.RESERVED_WORD },
            { "INC",       TokenType.RESERVED_WORD },
            { "READ",      TokenType.RESERVED_WORD },
            { "READLN",    TokenType.RESERVED_WORD },
            { "WRITE",     TokenType.RESERVED_WORD },
            { "WRITELN",   TokenType.RESERVED_WORD },
            
            //OneSymbolTokens
            { ";", TokenType.SPECIAL_SYMBOL       },
            { ":", TokenType.SPECIAL_SYMBOL       },
            { "=", TokenType.COMPARISON_OPERATOR  },
            { "(", TokenType.SPECIAL_SYMBOL       },
            { ")", TokenType.SPECIAL_SYMBOL       },
            { "+", TokenType.ARITHMETIC_OPERATOR  },
            { "-", TokenType.ARITHMETIC_OPERATOR  },
            { "*", TokenType.ARITHMETIC_OPERATOR  },
            { "/", TokenType.ARITHMETIC_OPERATOR  },
            { ">", TokenType.COMPARISON_OPERATOR  },
            { "<", TokenType.COMPARISON_OPERATOR  },
            { ",", TokenType.SPECIAL_SYMBOL       },
            { ".", TokenType.SPECIAL_SYMBOL       },
            
            //TwoSymbolTokens
            { "<>", TokenType.COMPARISON_OPERATOR },
            { ":=", TokenType.ASSIGNMENT_OPERATOR },
            { ">=", TokenType.COMPARISON_OPERATOR },
            { "=<", TokenType.COMPARISON_OPERATOR }

        };
        
    }
}