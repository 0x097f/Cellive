namespace Cellive.Cil.Metadata.Tables
{
    public sealed class AssemblyRow
    {
        public uint HashAlgId { get; set; }
        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }
        public ushort BuildNumber { get; set; }
        public ushort RevisionNumber { get; set; }
        public uint Flags { get; set; }
        public uint PublicKeyIndex { get; set; }
        public uint NameIndex { get; set; }
        public uint CultureIndex { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Culture { get; set; } = string.Empty;
        public byte[] PublicKey { get; set; } = System.Array.Empty<byte>();

        public System.Version Version => new(MajorVersion, MinorVersion, BuildNumber, RevisionNumber);

        public override string ToString() => $"{Name}, Version={Version}";
    }
}