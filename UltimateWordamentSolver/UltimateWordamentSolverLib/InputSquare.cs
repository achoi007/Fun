using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateWordamentSolverLib
{
    public class InputSquare
    {
        public enum PositionType
        {
            Normal,
            Prefix,
            Suffix,
        };

        public InputSquare()
        {
            IsHighPriority = false;
            IsOr = false;
            Position = PositionType.Normal;
        }

        public InputSquare(InputSquare inSq)
        {
            IsHighPriority = inSq.IsHighPriority;
            Position = inSq.Position;
            IsOr = inSq.IsOr;
            Letters = inSq.Letters;
            Letters2 = inSq.Letters2;
        }

        public bool IsHighPriority { get; set; }

        public PositionType Position { get; set; }

        public bool IsOr { get; set; }

        public string Letters { get; set; }

        public string Letters2 { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Position == PositionType.Suffix)
            {
                sb.Append("-");
            }

            if (IsHighPriority)
            {
                sb.Append(Letters.ToUpper());
            }
            else
            {
                sb.Append(Letters);
            }

            if (IsOr)
            {
                sb.Append("/");
                if (IsHighPriority)
                {
                    sb.Append(Letters2.ToUpper());
                }
                else
                {
                    sb.Append(Letters2);
                }
            }

            if (Position == PositionType.Prefix)
            {
                sb.Append("-");
            }

            return sb.ToString();
        }
    }
}
