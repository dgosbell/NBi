﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextMatchesRegex : AbstractTextPredicate
    {
        public TextMatchesRegex(bool not, object reference, StringComparison stringComparison)
            : base(not, reference, stringComparison)
        { }

        protected override bool Apply(object x)
        {
            var regexOption = StringComparison == StringComparison.InvariantCultureIgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
            var regex = new Regex(Reference.ToString(), regexOption);
            return regex.IsMatch(x.ToString());
        }

        public override string ToString()
        {
            return $"matches the regex '{Reference}'";
        }
    }
}
