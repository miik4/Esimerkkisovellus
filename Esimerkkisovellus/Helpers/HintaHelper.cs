using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esimerkkisovellus.Helpers
{
    public static class HintaHelper
    {

        public static bool OnkoMahdollinenHinta(string hinta)
        {
            double result;

            if (Double.TryParse(hinta.Replace(".", ","), out result))
            {
                return !(result <= 0);
            }
            return false;
        }

        public static double MuunnaHinta(string hinta)
        {
            var result = Double.Parse(hinta.Replace(".", ","));
            return result;
        }
    }
}
