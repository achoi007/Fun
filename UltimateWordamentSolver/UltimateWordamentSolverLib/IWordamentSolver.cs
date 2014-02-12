using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateWordamentSolverLib
{
    public interface IWordamentSolver
    {
        int MinLength { get; set; }

        int MinPoints { get; set; }

        WordSolutionOrder SortOrder { get; set; }

        IEnumerable<Word> Solve(IEnumerable<InputSquare> squares);
    }
}
