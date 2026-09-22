using Cellive.Cil.Metadata;

namespace Cellive.Cil.Definitions
{
    public sealed class CustomAttribute : ICustomAttribute
    {
        public MetadataToken Token { get; set; }

        public MetadataToken ParentToken { get; set; }
        public MetadataToken TypeToken { get; set; }

        public ITypeDef? AttributeType { get; set; }
        public byte[] RawValue { get; set; } = System.Array.Empty<byte>();

        public List<object> ConstructorArgumentsList { get; } = new();
        public Dictionary<string, object> NamedArgumentsDict { get; } = new();

        public IReadOnlyList<object> ConstructorArguments => ConstructorArgumentsList;
        public IReadOnlyDictionary<string, object> NamedArguments => NamedArgumentsDict;

        public bool HasConstructorArguments => ConstructorArgumentsList.Count > 0;
        public bool HasNamedArguments => NamedArgumentsDict.Count > 0;

        public override string ToString() =>
            $"CustomAttribute Type={TypeToken} ({ConstructorArgumentsList.Count} args)";
    }
}