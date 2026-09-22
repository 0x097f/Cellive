using Cellive.Cil.Metadata;
using Cellive.Executable;

namespace Cellive.Cil.Definitions
{
    public interface IModuleDef
    {
        MetadataToken Token { get; }
        string Name { get; }
        System.Guid Mvid { get; }
        System.Guid EncId { get; }
        System.Guid EncBaseId { get; }

        IAssemblyDef? Assembly { get; }
        PeImage? PeImage { get; }

        IReadOnlyList<ITypeDef> Types { get; }
        IReadOnlyList<IAssemblyRef> AssemblyReferences { get; }

        ITypeDef? FindType(string fullName);
        IReadOnlyList<ITypeDef> GetTypes();
    }

    public interface IAssemblyRef
    {
        string Name { get; }
        System.Version Version { get; }
        string Culture { get; }
        byte[] PublicKeyOrToken { get; }
        bool HasPublicKey { get; }
        bool IsRetargetable { get; }
    }
}