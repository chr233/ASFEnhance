using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASFEnhance.Other
{
    internal static class Response
    {
        internal static HashSet<string> Commands = new() {
            "CART",
            "CARTCOUNTRY",
            "CARTRESET",
        };
    }
}
