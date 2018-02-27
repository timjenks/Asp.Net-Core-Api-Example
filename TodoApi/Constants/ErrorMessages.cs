using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Constants
{
    public static class ErrorMessages
    {
        public const string TodoDueRequired = "Todo required a due date";

        public const string TodoDescriptionRequired = "Todo description is required";

        public const string TodoDescriptionMaxLength = 
            "Todo description can be of length " 
            + TodoLimits.MaxLengthString 
            + " at most";

    }
}
