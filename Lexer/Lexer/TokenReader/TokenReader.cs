using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lexer
{
    public static class TokenReader
    {
        private const int NUMBER_FIRST_LINE = 1;
        static string operatorsSymbols = "<>:=";
        public static List<Token> ReadTokensFromFile(string path)
        {
            List<Token> result = new List<Token>();
            int numberLine = NUMBER_FIRST_LINE;
            bool carriageInBlockComment = false; //carriage - каретка, курсор

            GetLinesFromFile(path).ForEach(line =>
            {
                CodeLine codeLine = new CodeLine(numberLine, line);
                result.AddRange(GetTokenListFromCodeLine(codeLine, ref carriageInBlockComment));

                numberLine++;
            });

            return result;
        }
        
        private static List<string> GetLinesFromFile(string path)
        {
            StreamReader sr = new StreamReader(path);
            return sr.ReadToEnd().Split('\n').ToList();
        }

        private static List<Token> GetTokenListFromCodeLine(CodeLine codeLine, ref bool carriageInBlockComment)
        {
            List<Token> result = new List<Token>();
            string line = codeLine.Text;

            bool carriageInLiteral, carriageInInlineСomment, wasCarriageInLiteral;
            carriageInLiteral = carriageInInlineСomment = wasCarriageInLiteral = false;

            string bufferSymbols, bufferLiteral = "";
            bufferSymbols = bufferLiteral = "";

            int quotesAmount = 0;
            char currSymbol;
            char nextSymbol;

            for (int i = 0; i < line.Length; i++)
            {
                currSymbol = line[i];
                nextSymbol = GetSymbolByIndex(line, i + 1);

                if (currSymbol == '\'') quotesAmount++;

                carriageInBlockComment = IsCarriageInBlockComment(currSymbol, carriageInBlockComment, carriageInLiteral);

                if (CarriageInLiteral(currSymbol, nextSymbol, carriageInLiteral, quotesAmount))
                {
                    carriageInLiteral = wasCarriageInLiteral = true;
                }
                else
                {
                    carriageInLiteral = false;
                    quotesAmount = 0;
                }
                
                if (!carriageInBlockComment && !carriageInLiteral && LineComment(currSymbol, nextSymbol)) return result;
                
                if(carriageInBlockComment) continue;
                
                if (carriageInLiteral)
                {
                    bufferLiteral += (quotesAmount == 1 && currSymbol == '\'') ? "" : currSymbol;
                    continue;
                }
                
                if (wasCarriageInLiteral)
                {
                    result.Add(new Token(bufferLiteral, TokenType.LITERAL, i - bufferLiteral.Length, codeLine.Index));
                    
                    bufferLiteral = "";
                    wasCarriageInLiteral = false;
                }

                

                ProcessSymbols(currSymbol, nextSymbol, ref bufferSymbols, ref i, ref result, codeLine.Index);

            }
            if (bufferSymbols.Length > 0)
            {
                result.Add(TokenBuilder.Construct(bufferSymbols, codeLine.Index, 0));
            }
            
            
            return result;
        }

        private static void ProcessSymbols(char currCh, char nextCh, ref string bufferCh, ref int i, ref List<Token> tokens, int numLine)
        {
            if ((char.IsDigit(currCh) || char.IsLetter(currCh))) bufferCh += $"{currCh}";
            
            else if (currCh != '\'' && currCh != '{' && currCh != '/' && currCh != '}')
            {
                if (bufferCh.Length > 0) tokens.Add(TokenBuilder.Construct(bufferCh, numLine, i - bufferCh.Length));
                
                if (currCh != ' ')
                {
                    if (!operatorsSymbols.Contains(currCh))
                    {
                        tokens.Add(TokenBuilder.Construct($"{currCh}", numLine, i));
                    }
                    
                    else if (operatorsSymbols.Contains(nextCh))
                    {
                        tokens.Add(TokenBuilder.Construct($"{currCh}{nextCh}", numLine, i ));
                        i++;
                    }
                    else
                    {
                        tokens.Add(TokenBuilder.Construct($"{currCh}", numLine, i));
                    }
                }
                bufferCh = "";
            }


        }

        private static bool LineComment(char currSymbol, char nextSymbol) => currSymbol == '/' && nextSymbol == '/';

        private static char GetSymbolByIndex(string from, int index) =>
            (0 <= index && index < from.Length) ? from[index] : '\0';


        private static bool IsCarriageInBlockComment(char ch, bool carriageInBlockComment, bool carriageInLiteral)
        { 
            if (carriageInLiteral) return carriageInBlockComment;
            if (ch == '{')         return true;
            if (ch == '}')         return false;
                                   return carriageInBlockComment;
        }

        private static bool CarriageInLiteral(char currSymbol, char nextSymbol, bool carriageInLiteral, int quotesAmount)
        {
            if (currSymbol == '\'')
            {
                if (quotesAmount % 2 == 0 && nextSymbol != '\'')
                    return false;
                
                return true;
            }
            
            return carriageInLiteral;
        }
    }
}
