using CilDotNet.Cil.Metadata;
using CilDotNet.Executable;

namespace CilDotNet.Cil.Definitions
{
    public sealed class ModuleDef : IModuleDef
    {
        public MetadataToken Token { get; set; }
        public string Name { get; set; } = string.Empty;
        public System.Guid Mvid { get; set; }
        public System.Guid EncId { get; set; }
        public System.Guid EncBaseId { get; set; }

        public IAssemblyDef? Assembly { get; set; }
        public PeImage? PeImage { get; set; }

        public List<ITypeDef> TypesList { get; } = new();
        public List<IAssemblyRef> AssemblyReferencesList { get; } = new();

        public IReadOnlyList<ITypeDef> Types => TypesList;
        public IReadOnlyList<IAssemblyRef> AssemblyReferences => AssemblyReferencesList;

        public ITypeDef? FindType(string fullName)
        {
            foreach (var type in TypesList)
            {
                if (type.FullName == fullName)
                    return type;
            }
            return null;
        }

        public IReadOnlyList<ITypeDef> GetTypes() => TypesList;

        public override string ToString() => Name;
    }
}