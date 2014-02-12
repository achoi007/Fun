using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateWordamentSolverLib
{
    public static class WordamentSolver
    {
        public static IEnumerable<Word> Solve(this IWordamentSolver solver, string inString)
        {
            List<InputSquare> squares = new List<InputSquare>();

            int strLen = inString.Length;
            bool isSpecial = false;
            InputSquare currSq = null;
            StringBuilder currLetters = null;
            bool isDoingOr = false;

            for (int i = 0; i < strLen; i++)
            {
                if (isSpecial)
                {
                    char ch = inString[i];

                    if (ch == ' ')
                    {
                        if (currLetters.Length > 0)
                        {
                            // Add letters to current square
                            string letters = currLetters.ToString();
                            if (isDoingOr)
                            {
                                currSq.Letters2 = letters;
                            }
                            else
                            {
                                currSq.Letters = letters;
                            }

                            // Automatic mark special square as high priority
                            currSq.IsHighPriority = true;

                            // Add current square to list of squares
                            squares.Add(currSq);
                        }

                        // Reset to not special processing
                        isSpecial = false;
                        isDoingOr = false;
                    }
                    else
                    {
                        if (ch == '-')
                        {
                            // Figure out if prefix or suffix based on whether there is any previous
                            // character in current word.
                            if (currLetters.Length == 0)
                            {
                                currSq.Position = InputSquare.PositionType.Suffix;
                            }
                            else
                            {
                                currSq.Position = InputSquare.PositionType.Prefix;
                            }
                        }
                        else if (ch == '/')
                        {
                            // Mark current square as OR square
                            currSq.IsOr = true;
                            
                            // Everything before / forms first "or" choice
                            currSq.Letters = currLetters.ToString();
                            currLetters.Clear();

                            // So that ending " " processing will know to everything after / into Letters2
                            isDoingOr = true;
                        }
                        else
                        {
                            currLetters.Append(char.ToLower(ch));
                        }
                    }
                }
                else
                {
                    // Not processing special characters
                    char ch = inString[i];
                    if (ch == ' ')
                    {
                        isSpecial = true;
                        currSq = new InputSquare();
                        currLetters = new StringBuilder();
                    }
                    else
                    {
                        bool isHighPriority = false;
                        if (char.IsUpper(ch))
                        {
                            isHighPriority = true;
                            ch = char.ToLower(ch);
                        }
                        squares.Add(new InputSquare()
                        {
                            Letters = ch.ToString(),
                            IsHighPriority = isHighPriority
                        });
                    }
                }
            }

            return solver.Solve(squares);
        }
    }
}
