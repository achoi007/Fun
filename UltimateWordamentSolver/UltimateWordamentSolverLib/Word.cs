using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateWordamentSolverLib
{
    public class Word
    {
        private List<OutputSquare> mSource = new List<OutputSquare>();

        public Word()
        {
            Points = 0;
            WordString = string.Empty;
        }

        public int Points { get; private set; }

        public string WordString { get; private set; }

        public IEnumerable<OutputSquare> Source
        {
            get { return mSource; }

            set
            {
                mSource.Clear();
                mSource.AddRange(value);

                // Update cached values based on new squares
                Points = mSource.Sum(i => i.Points);
                WordString = mSource.Aggregate(new StringBuilder(),
                    (acc, sq) => acc.Append(sq.Letters),
                    sq => sq.ToString());
            }
        }
    }
}
