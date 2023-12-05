using System;
using System.Collections.Generic;

namespace Universe
{
    public static class UnitConversion
    {
        public static string NumberFormat = "#.#####";
        public static double Convert(double d, DistanceUnit from, DistanceUnit to)
        {
            return d * ConversionToKM[from] / ConversionToKM[to];
        }

        private static readonly IReadOnlyDictionary<DistanceUnit, double> ConversionToKM = new Dictionary<DistanceUnit, double>()
        {
            { DistanceUnit.None, 0 },
            { DistanceUnit.PlanckLength, 1.6e-38 },
            { DistanceUnit.HydrogenAtom, 2.50e-14 },
            { DistanceUnit.NanoMetres, 1e-12},
            { DistanceUnit.Millimetres, 1e-6 },
            { DistanceUnit.Centimetres, 1e-5 },
            { DistanceUnit.Metres, 1e-3},
            { DistanceUnit.Kilometers, 1 },
            { DistanceUnit.LightYear, 9.461e+12 },
        };

        public static DistanceUnit ReasonableFormat(double km)
        {
            km = Math.Abs(km);
            if (km < double.Epsilon)
                return DistanceUnit.None;
            var DistanceValues = Enum.GetValues(typeof(DistanceUnit));

            for (int i = DistanceValues.Length - 1; i >= 0; i--)
            {
                var DistanceUnitCurrent = (DistanceUnit)DistanceValues.GetValue(i);
                if (km >= ConversionToKM[DistanceUnitCurrent])
                    return DistanceUnitCurrent;
            }
            return (DistanceUnit)DistanceValues.GetValue(0);
        }

        public static string ToAbbreviation(DistanceUnit unit)
        {
            return unit switch
            {
                DistanceUnit.None => "",
                DistanceUnit.PlanckLength => "Planck Lengths",
                DistanceUnit.HydrogenAtom => "Hydrogen Atoms",
                DistanceUnit.NanoMetres => "nm",
                DistanceUnit.Millimetres => "mm",
                DistanceUnit.Centimetres => "cm",
                DistanceUnit.Metres => "m",
                DistanceUnit.Kilometers => "km",
                DistanceUnit.LightYear => "ly",
                _ => throw new NotImplementedException(),
            };
        }

        public static string ToCleanString(this double d)
        {
            if (d < 0)
                return "-" + ToCleanString(Math.Abs(d));

            if (d == 0)
                return "0";

            if (d < 5)
                return d.ToString(NumberFormat);

            if (d > 1e15)
                return d.ToString();

            if (double.IsNaN(d))
                return "NaN";

            return AddCommas((d - d % 1).ToString(NumberFormat));
        }

        private static string AddCommas(string s)
        {
            string newStr = "";

            for (int i = s.Length - 1; i >= 0; i--)
            {
                if ((s.Length - i) % 3 == 0)
                    newStr = "," + s[i] + newStr;
                else
                    newStr = s[i] + newStr;
            }

            if (newStr.Length == 0)
                return "";

            if (newStr[0] == ',')
                return newStr[1..];

            if (newStr[0] == '-' && newStr[1] == ',')
                newStr = "-" + newStr[2..];

            return newStr;
        }
    }

    public enum DistanceUnit
    {
        None,
        PlanckLength,
        HydrogenAtom,
        NanoMetres,
        Millimetres,
        Centimetres,
        Metres,
        Kilometers,
        LightYear
    }
}