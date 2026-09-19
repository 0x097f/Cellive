namespace CilDotNet.Executable
{
    public sealed class CliHeader
    {
        public const uint ExpectedCb = 72;

        public uint Cb { get; set; }
        public ushort MajorRuntimeVersion { get; set; }
        public ushort MinorRuntimeVersion { get; set; }
        public DataDirectory MetaData { get; set; }
        public uint Flags { get; set; }
        public uint EntryPointToken { get; set; }
        public DataDirectory Resources { get; set; }
        public DataDirectory StrongNameSignature { get; set; }
        public DataDirectory CodeManagerTable { get; set; }
        public DataDirectory VtableFixups { get; set; }
        public DataDirectory ExportAddressTableJumps { get; set; }
        public DataDirectory ManagedNativeHeader { get; set; }

        public bool HasMetadata => !MetaData.IsEmpty;
        public bool HasResources => !Resources.IsEmpty;
        public bool HasStrongName => !StrongNameSignature.IsEmpty;

        public static CliHeader Read(BinaryReader reader)
        {
            return new CliHeader
            {
                Cb = reader.ReadUInt32(),
                MajorRuntimeVersion = reader.ReadUInt16(),
                MinorRuntimeVersion = reader.ReadUInt16(),
                MetaData = DataDirectory.Read(reader),
                Flags = reader.ReadUInt32(),
                EntryPointToken = reader.ReadUInt32(),
                Resources = DataDirectory.Read(reader),
                StrongNameSignature = DataDirectory.Read(reader),
                CodeManagerTable = DataDirectory.Read(reader),
                VtableFixups = DataDirectory.Read(reader),
                ExportAddressTableJumps = DataDirectory.Read(reader),
                ManagedNativeHeader = DataDirectory.Read(reader),
            };
        }

        public bool IsCorFlagSet(CorFlags flag) => (Flags & (uint)flag) != 0;

        public override string ToString() =>
            $"CLI Header v{MajorRuntimeVersion}.{MinorRuntimeVersion} " +
            $"Flags=0x{Flags:X8} EntryPoint=0x{EntryPointToken:X8}";
    }
}