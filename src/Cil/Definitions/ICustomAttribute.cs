using Cellive.Cil.Metadata;

namespace Cellive.Cil.Definitions
{
    public interface ICustomAttribute
    {
        MetadataToken Token { get; }

        MetadataToken ParentToken { get; }
        MetadataToken TypeToken { get; }

        ITypeDef? AttributeType { get; }
        byte[] RawValue { get; }

        IReadOnlyList<object> ConstructorArguments { get; }
        IReadOnlyDictionary<string, object> NamedArguments { get; }

        bool HasConstructorArguments { get; }
        bool HasNamedArguments { get; }
    }
}