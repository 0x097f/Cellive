using CilDotNet.Cil.Metadata;

namespace CilDotNet.Cil.Definitions
{
    public sealed class AssemblyDef : IAssemblyDef
    {
        public MetadataToken Token { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FullName => $"{Name}, Version={Version}, Culture={Culture}, PublicKeyToken={PublicKeyToken}";
        public System.Version Version { get; set; } = new(0, 0, 0, 0);
        public string Culture { get; set; } = string.Empty;
        public byte[] PublicKey { get; set; } = System.Array.Empty<byte>();
        public uint Flags { get; set; }
        public uint HashAlgId { get; set; }

        public bool HasPublicKey => (Flags & 0x0001) != 0;
        public bool IsRetargetable => (Flags & 0x0100) != 0;

        public string PublicKeyToken
        {
            get
            {
                if (PublicKey == null || PublicKey.Length == 0)
                    return "null";
                return System.BitConverter.ToString(PublicKey).Replace("-", "").ToLowerInvariant();
            }
        }

        public List<IModuleDef> ModulesList { get; } = new();
        public List<ICustomAttribute> CustomAttributesList { get; } = new();

        public IReadOnlyList<IModuleDef> Modules => ModulesList;
        public IReadOnlyList<ICustomAttribute> CustomAttributes => CustomAttributesList;

        public override string ToString() => FullName;
    }
}