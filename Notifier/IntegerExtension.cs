using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notifier
{
    public static class IntegerExtension
    {
        public static string es(this int i)
        {
            return i > 1 ? "es" : "";
        }

        public static string Has(this int i)
        {
            return i > 1 ? "have" : "has";
        }

        public static string Is(this int i)
        {
            return i > 1 ? "are" : "is";
        }

        public static string s(this int i)
        {
            return i > 1 ? "s" : "";
        }
    }
}
