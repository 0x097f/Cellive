namespace Cellive.Cil.Metadata.Signatures
{
    public sealed class PropertySignature
    {
        public bool HasThis { get; set; }
        public int ParameterCount { get; set; }
        public TypeSignature? PropertyType { get; set; }

        public List<TypeSignature> Parameters { get; } = new();

        public override string ToString()
        {
            var typeName = PropertyType?.FullName ?? "?";
            var paramNames = string.Join(", ", Parameters.ConvertAll(p => p.FullName));
            return $"{typeName} ({paramNames})";
        }
    }
}