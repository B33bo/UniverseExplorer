using System;

namespace Universe
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AnimalAttribute : Attribute
    {
        public Type SpeciesType { get; set; }
    }
}
