namespace Universe.Inspector
{
    [System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class InspectableVarAttribute : System.Attribute
    {
        public string VariableName;
        public InspectableVarAttribute(string variableName)
        {
            VariableName = variableName;
        }

        public object[] Params { get; set; }
    }
}
