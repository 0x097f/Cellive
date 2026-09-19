namespace CilDotNet.Cil.Definitions
{
    public sealed class LocalVariable : ILocalVariable
    {
        public int Index { get; set; }
        public string? Name { get; set; }
        public ITypeDef? VariableType { get; set; }

        public bool IsPinned { get; set; }
        public bool IsByReference { get; set; }

        public bool HasName => !string.IsNullOrEmpty(Name);

        public override string ToString() =>
            HasName ? $"{Name} : {VariableType?.Name ?? "?"}" : $"v{Index} : {VariableType?.Name ?? "?"}";
    }
}