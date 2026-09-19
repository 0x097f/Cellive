namespace CilDotNet.Cil.Metadata.Signatures
{
    public sealed class TypeSignature
    {
        public ElementType ElementType { get; set; }
        public string? Name { get; set; }
        public string? Namespace { get; set; }

        public TypeSignature? ElementTypeSignature { get; set; }
        public TypeSignature? DeclaringType { get; set; }

        public bool IsValueType { get; set; }
        public bool IsByReference { get; set; }
        public bool IsPointer { get; set; }
        public bool IsPinned { get; set; }
        public bool IsArray { get; set; }
        public bool IsSzArray { get; set; }
        public bool IsGenericInstance { get; set; }
        public bool IsGenericParameter { get; set; }
        public bool IsMethodGenericParameter { get; set; }

        public int Rank { get; set; }
        public int GenericParameterIndex { get; set; }

        public List<TypeSignature> GenericArguments { get; } = new();
        public List<TypeSignature> Modifiers { get; } = new();
        public List<int> Sizes { get; } = new();
        public List<int> LowerBounds { get; } = new();

        public string FullName
        {
            get
            {
                if (IsByReference)
                    return (ElementTypeSignature?.FullName ?? "?") + "&";
                if (IsPointer)
                    return (ElementTypeSignature?.FullName ?? "?") + "*";
                if (IsArray)
                {
                    var element = ElementTypeSignature?.FullName ?? "?";
                    if (Rank <= 1)
                        return element + "[]";
                    return element + "[" + new string(',', Rank - 1) + "]";
                }
                if (IsSzArray)
                    return (ElementTypeSignature?.FullName ?? "?") + "[]";
                if (IsGenericInstance)
                {
                    var baseName = string.IsNullOrEmpty(Namespace) ? Name : Namespace + "." + Name;
                    var args = string.Join(", ", GenericArguments.ConvertAll(a => a.FullName));
                    return baseName + "<" + args + ">";
                }
                if (IsGenericParameter)
                    return IsMethodGenericParameter ? "!!" + GenericParameterIndex : "!" + GenericParameterIndex;

                return string.IsNullOrEmpty(Namespace) ? Name ?? "?" : Namespace + "." + Name;
            }
        }

        public override string ToString() => FullName;
    }
}