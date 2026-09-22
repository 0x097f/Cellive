using Cellive.Cil.Metadata;

namespace Cellive.Cil.Definitions
{
    public interface IAssemblyDef
    {
        MetadataToken Token { get; }
        string Name { get; }
        string FullName { get; }
        System.Version Version { get; }
        string Culture { get; }
        byte[] PublicKey { get; }
        uint Flags { get; }
        uint HashAlgId { get; }

        bool HasPublicKey { get; }
        bool IsRetargetable { get; }

        IReadOnlyList<IModuleDef> Modules { get; }
        IReadOnlyList<ICustomAttribute> CustomAttributes { get; }
    }
}