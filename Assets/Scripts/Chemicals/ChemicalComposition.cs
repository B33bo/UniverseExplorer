using UnityEngine;
using System.Collections.Generic;

namespace Universe
{
    public struct ChemicalComposition
    {
        public Chemical[] chemicals;
        public BondingType bondingType;
        public float[] weights;

        public Chemical this[int index]
        {
            get => chemicals[index];
            set => chemicals[index] = value;
        }

        public ChemicalComposition(BondingType bondingType, params Chemical[] chemicals)
        {
            this.bondingType = bondingType;
            this.chemicals = chemicals;

            weights = new float[chemicals.Length];
            for (int i = 0; i < chemicals.Length; i++)
                weights[i] = chemicals[i].weight;
        }

        public enum BondingType : byte
        {
            Ionic,
            Covalent,
        }
    }
}
