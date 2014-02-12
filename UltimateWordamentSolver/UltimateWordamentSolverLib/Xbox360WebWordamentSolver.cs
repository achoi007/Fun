﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UltimateWordamentSolverLib
{
    public class Xbox360WebWordamentSolver : IWordamentSolver
    {
        public int MinLength
        {
            get;
            set;
        }

        public int MinPoints
        {
            get;
            set;
        }

        public WordSolutionOrder SortOrder
        {
            get;
            set;
        }

        /// <summary>
        /// URL is of the form:
        /// http://dev.360surface.net/Wordament/EnterBoard.aspx?BoardID=p0g0e0d0l0l0d0st0c0f0i0e0s0h0o0n0&Length=3&Lang=English&Sort=by%20points&Show=not%20showing
        /// </summary>
        /// <param name="squares"></param>
        /// <returns></returns>
        public IEnumerable<Word> Solve(IEnumerable<InputSquare> squares)
        {
            if (squares.Any(sq => sq.IsOr))
            {
                // Underlying service does not support OR, need to send 2 resquests and then combine results

                List<InputSquare> squares1 = new List<InputSquare>();
                List<InputSquare> squares2 = new List<InputSquare>();
                int fakePos = -1;
                int pos = 0;

                foreach (var square in squares)
                {
                    if (square.IsOr)
                    {
                        // Remember the squares being replaced.  Will be used in processing later to substitute back original square
                        fakePos = pos;

                        // square1 gets first letters, square 2 gets second letters
                        squares1.Add(new InputSquare(square) { IsOr = false });
                        squares2.Add(new InputSquare(square) { IsOr = false, Letters = square.Letters });
                    }
                    else
                    {
                        // Not an OR, just add it
                        squares1.Add(square);
                        squares2.Add(square);
                    }

                    ++pos;
                }

                // Generate 2 web requests, wait for both web requests to be finished

                WebRequest webReq1 = GenerateWebRequest(squares1);
                WebRequest webReq2 = GenerateWebRequest(squares2);
                WebResponse webRes1, webRes2;
                GetWebResponses(webReq1, webReq2, out webRes1, out webRes2);

                // Replace fake input squares inserted above with original square
                InputSquare origSq = squares.ElementAt(fakePos);
                squares1.RemoveAt(fakePos);
                squares1.Insert(fakePos, origSq);
                squares2.RemoveAt(fakePos);
                squares2.Insert(fakePos, origSq);

                // Combine into single list, eliminate duplicates
                var words1 = ParseResponse(squares1, webRes1);
                var words2 = ParseResponse(squares2, webRes2);
                var words = WordamentUtils.Combine(words1, words2, SortOrder);

                return words;
            }
            else
            {
                WebRequest webReq = GenerateWebRequest(squares);
                WebResponse webRes = webReq.GetResponse();
                return ParseResponse(squares, webRes);
            }
        }

        private void GetWebResponses(WebRequest webReq1, WebRequest webReq2, out WebResponse webRes1, out WebResponse webRes2)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Word> ParseResponse(IEnumerable<InputSquare> squares, WebResponse webRes)
        {
            throw new NotImplementedException();
        }

        private WebRequest GenerateWebRequest(IEnumerable<InputSquare> squares)
        {
            throw new NotImplementedException();
        }

    }
}
