using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using UltimateWordamentSolverLib;
using System.Linq;
using System.Collections.Generic;

namespace LibTest
{
    [TestClass]
    public class WordamentSolverTest
    {
        private List<InputSquare> mInSquares;

        private IWordamentSolver mSolver;

        private IEnumerable<Word> SolveStub(IEnumerable<InputSquare> squares)
        {
            mInSquares = squares.ToList();
            return Enumerable.Empty<Word>();
        }

        [TestInitialize]
        public void SetUp()
        {
            mInSquares = new List<InputSquare>();

            mSolver = new UltimateWordamentSolverLib.Fakes.StubIWordamentSolver()
            {
                SolveIEnumerableOfInputSquare = inSq => SolveStub(inSq)
            };
        }

        [TestMethod]
        public void SimpleSolveStringTest()
        {
            string s = "abcdefghijklmnop";
            mSolver.Solve(s);

            Assert.AreEqual(s.Length, mInSquares.Count);
            foreach (var pair in s.Zip(mInSquares, (ch, inSq) => Tuple.Create(ch, inSq)))
            {
                InputSquare inSq = pair.Item2;
                Assert.AreEqual(pair.Item1.ToString(), inSq.Letters);
                Assert.IsFalse(inSq.IsHighPriority);
                Assert.IsFalse(inSq.IsOr);
                Assert.AreEqual(InputSquare.PositionType.Normal, inSq.Position);
            }
        }

        [TestMethod]
        public void SimpleWithSpaceSolveStringTest()
        {
            string s = "abcdef G hijklm n oP";
            mSolver.Solve(s);

            Assert.AreEqual(16, mInSquares.Count);
            foreach (var pair in s.Where(c => c != ' ').Zip(mInSquares, (ch, inSq) => Tuple.Create(char.ToLower(ch), inSq)))
            {
                InputSquare inSq = pair.Item2;

                Assert.AreEqual(pair.Item1.ToString(), inSq.Letters);
                Assert.IsFalse(inSq.IsOr);
                Assert.AreEqual(InputSquare.PositionType.Normal, inSq.Position);

                if (pair.Item1 == 'g' || pair.Item1 == 'p')
                {
                    Assert.IsTrue(inSq.IsHighPriority);

                }
                else if (pair.Item1 == 'n')
                {
                    // Automatic high priority
                    Assert.IsTrue(inSq.IsHighPriority);
                }
            }
        }

        [TestMethod]
        public void OrSolveStringTest()
        {
            string s = "ab c1/c2 defg hxx/hyy ijklmnop";
            mSolver.Solve(s);

            Assert.AreEqual(16, mInSquares.Count);
            Assert.IsTrue(mInSquares[2].IsOr);
            Assert.AreEqual("c1", mInSquares[2].Letters);
            Assert.AreEqual("c2", mInSquares[2].Letters2);

            Assert.IsTrue(mInSquares[7].IsOr);
            Assert.AreEqual("hxx", mInSquares[7].Letters);
            Assert.AreEqual("hyy", mInSquares[7].Letters2);

            for (int pos = 0; pos < mInSquares.Count; pos++)
            {
                if (pos != 2 && pos != 7)
                {
                    Assert.IsFalse(mInSquares[pos].IsOr);
                    Assert.IsFalse(mInSquares[pos].IsHighPriority);
                }
                else
                {
                    Assert.IsTrue(mInSquares[pos].IsHighPriority);
                }
            }
        }

        [TestMethod]
        public void PrefixSolveStringTest()
        {
            string s = "abc d12- efghijklmnop";
            mSolver.Solve(s);

            InputSquare prefixSq = mInSquares[3];
            Assert.AreEqual(16, mInSquares.Count);
            Assert.AreEqual(InputSquare.PositionType.Prefix, prefixSq.Position);
            Assert.IsTrue(prefixSq.IsHighPriority);
            Assert.AreEqual("d12", prefixSq.Letters);

            foreach (var inSq in mInSquares.Where(i => i != prefixSq))
            {
                Assert.IsFalse(inSq.IsHighPriority);
                Assert.AreEqual(InputSquare.PositionType.Normal, inSq.Position);
            }
        }

        [TestMethod]
        public void SuffixSolveStringTest()
        {
            string s = "abcde -f111 ghijklmnop";
            mSolver.Solve(s);

            InputSquare suffixSq = mInSquares[5];
            Assert.AreEqual(16, mInSquares.Count);
            Assert.AreEqual(InputSquare.PositionType.Suffix, suffixSq.Position);
            Assert.IsTrue(suffixSq.IsHighPriority);
            Assert.AreEqual("f111", suffixSq.Letters);

            foreach (var inSq in mInSquares.Where(i => i != suffixSq))
            {
                Assert.IsFalse(inSq.IsHighPriority);
                Assert.AreEqual(InputSquare.PositionType.Normal, inSq.Position);
            }
        }
    }
}
