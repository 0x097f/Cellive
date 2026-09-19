namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class AssemblyRefRow
    {
        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }
        public ushort BuildNumber { get; set; }
        public ushort RevisionNumber { get; set; }
        public uint Flags { get; set; }
        public uint PublicKeyOrTokenIndex { get; set; }
        public uint NameIndex { get; set; }
        public uint CultureIndex { get; set; }
        public uint HashValueIndex { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Culture { get; set; } = string.Empty;
        public byte[] PublicKeyOrToken { get; set; } = System.Array.Empty<byte>();
        public byte[] HashValue { get; set; } = System.Array.Empty<byte>();

        public System.Version Version => new(MajorVersion, MinorVersion, BuildNumber, RevisionNumber);

        public bool HasPublicKey => (Flags & 0x0001) != 0;
        public bool IsRetargetable => (Flags & 0x0100) != 0;

        public override string ToString() => $"{Name}, Version={Version}";
    }
}