﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Scalar.Comparer
{
    public class NumericAbsoluteTolerance : NumericTolerance
    {
        public override string ValueString
        {
            get
            {
                switch (Side)
                {
                    case SideTolerance.More:
                        return string.Format("+{0}",base.ValueString);
                    case SideTolerance.Less:
                        return string.Format("-{0}", base.ValueString);
                }
                return base.ValueString;
            }
        }


        public NumericAbsoluteTolerance(decimal value, SideTolerance side)
            : base(value, side)
        {
            Value = value;
        }

        public static NumericAbsoluteTolerance None
        {
            get
            {
                if (none == null)
                    none = new NumericAbsoluteTolerance(0, SideTolerance.Both);
                return none;
            }
        }

        private static NumericAbsoluteTolerance none;
    }
}
