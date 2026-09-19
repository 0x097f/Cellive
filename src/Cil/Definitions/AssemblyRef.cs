namespace CilDotNet.Cil.Definitions
{
    public sealed class AssemblyRef : IAssemblyRef
    {
        public string Name { get; set; } = string.Empty;
        public System.Version Version { get; set; } = new(0, 0, 0, 0);
        public string Culture { get; set; } = string.Empty;
        public byte[] PublicKeyOrToken { get; set; } = System.Array.Empty<byte>();

        public uint Flags { get; set; }

        public bool HasPublicKey => (Flags & 0x0001) != 0;
        public bool IsRetargetable => (Flags & 0x0100) != 0;

        public override string ToString() => $"{Name}, Version={Version}";
    }
}