using UnityEngine;

namespace Universe
{
    public struct Ion
    {
        public static Ion[] PeriodicTable
        {
            get
            {
                if (_periodicTable is null)
                    _periodicTable = CSVreader.GetValues<Ion>(Resources.Load<TextAsset>("PeriodicTable").text.Split('\n'));

                return _periodicTable;
            }
        }

        public static Ion Parse(string ionString)
        {
            //ionString example:
            //H = hydrogen
            //H+2 = hydrogen with positive charge (2 electrons lost)

            if (ionString.Length <= 2)
                return FindIon(ionString);

            string element = ionString[0].ToString();
            if (ionString[1] >= 'a' && ionString[1] <= 'z')
                element += ionString[1];

            Ion ion = FindIon(element);

            //positive charge = less electrons, since electrons have a charge of -1
            ion.NumberofElectrons -= int.Parse(ionString.Substring(element.Length));
            return ion;
        }

        public static Ion FindIon(string symbol)
        {
            for (int i = 0; i < PeriodicTable.Length; i++)
            {
                if (PeriodicTable[i].Symbol == symbol)
                    return PeriodicTable[i];
            }
            return Unknowninium;
        }

        private Ion(string ElementName, string symbol)
        {
            AtomicNumber = -1;
            Element = ElementName;
            Symbol = symbol;
            AtomicMass = double.NaN;
            NumberofNeutrons = 0;
            NumberofProtons = 0;
            NumberofElectrons = 0;
            Period = 0;
            Group = 0;
            Phase = PhaseType.unknown;
            Radioactive = false;
            Natural = false;
            Metal = false;
            Nonmetal = false;
            Metalloid = false;
            Type = "";
            AtomicRadius = double.NaN;
            Electronegativity = double.NaN;
            FirstIonization = double.NaN;
            Density = double.NaN;
            MeltingPoint = double.NaN;
            BoilingPoint = double.NaN;
            NumberOfIsotopes = 0;
            Discoverer = "";
            Year = 0;
            SpecificHeat = double.NaN;
            NumberofShells = 0;
            NumberofValence = 0;
        }

        public static readonly Ion Unknowninium = new Ion("Unknowninium", "??");

        private static Ion[] _periodicTable;

        public int AtomicNumber;
        public string Element;
        public string Symbol;
        public double AtomicMass;
        public int NumberofNeutrons;
        public int NumberofProtons;
        public int NumberofElectrons;
        public int Period;
        public int Group;
        public PhaseType Phase;
        public bool Radioactive;
        public bool Natural;
        public bool Metal;
        public bool Nonmetal;
        public bool Metalloid;
        public string Type;
        public double AtomicRadius;
        public double Electronegativity;
        public double FirstIonization;
        public double Density;
        public double MeltingPoint;
        public double BoilingPoint;
        public int NumberOfIsotopes;
        public string Discoverer;
        public int Year;
        public double SpecificHeat;
        public int NumberofShells;
        public int NumberofValence;

        public int Charge
        {
            get => NumberofProtons - NumberofElectrons;
        }

        public override string ToString()
        {
            if (Charge == 0)
                return $"{Element} ({Symbol})";
            if (Charge > 0)
                return $"{Element} ({Symbol}+{Charge})";
            return $"{Element} ({Symbol}{Charge})";
        }

        public enum PhaseType : byte
        {
            solid = 0,
            liquid = 1,
            liq = 1,
            gas = 2,
            plasma = 3,
            artificial = 4,
            unknown = 5,
        }
    }
}
