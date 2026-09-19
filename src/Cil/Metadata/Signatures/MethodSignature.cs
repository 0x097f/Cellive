namespace CilDotNet.Cil.Metadata.Signatures
{
    public sealed class MethodSignature
    {
        public bool HasThis { get; set; }
        public bool HasExplicitThis { get; set; }
        public bool IsGeneric { get; set; }
        public bool IsVarArg { get; set; }

        public int GenericParameterCount { get; set; }
        public int ParameterCount { get; set; }

        public TypeSignature? ReturnType { get; set; }

        public List<TypeSignature> Parameters { get; } = new();
        public List<TypeSignature> SentinelParameters { get; } = new();

        public override string ToString()
        {
            var returnName = ReturnType?.FullName ?? "?";
            var paramNames = string.Join(", ", Parameters.ConvertAll(p => p.FullName));
            return $"{returnName} ({paramNames})";
        }
    }
}