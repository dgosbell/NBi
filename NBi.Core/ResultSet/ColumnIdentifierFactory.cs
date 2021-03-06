﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    public class ColumnIdentifierFactory
    {
        public IColumnIdentifier Instantiate(string identifier)
        {
            identifier = identifier.Trim();
            if (identifier.StartsWith("#"))
            {
                var positionString = identifier.Substring(1);
                if (int.TryParse(positionString, out var position))
                    if (position>=0)
                        return new ColumnOrdinalIdentifier(position);
                throw new ArgumentException($"The column identification '{positionString}' is starting by a '#' implying that it's a position but the position is not a numeric value or not a positive value or not an integer value.");
            }
            else
            {
                if (identifier.StartsWith("[") && identifier.EndsWith("]"))
                    return new ColumnNameIdentifier(identifier.Substring(1, identifier.Length - 2));
                else
                    return new ColumnNameIdentifier(identifier);
            }
        }
    }
}
