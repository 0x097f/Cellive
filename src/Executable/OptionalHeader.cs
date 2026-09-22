namespace Cellive.Executable
{
    public sealed class OptionalHeader
    {
        public const ushort MagicPe32 = 0x010B;
        public const ushort MagicPe32Plus = 0x020B;

        public ushort Magic { get; set; }
        public byte MajorLinkerVersion { get; set; }
        public byte MinorLinkerVersion { get; set; }
        public uint SizeOfCode { get; set; }
        public uint SizeOfInitializedData { get; set; }
        public uint SizeOfUninitializedData { get; set; }
        public uint AddressOfEntryPoint { get; set; }
        public uint BaseOfCode { get; set; }
        public uint BaseOfData { get; set; }         //Only PE32
        public ulong ImageBase { get; set; }
        public uint SectionAlignment { get; set; }
        public uint FileAlignment { get; set; }
        public ushort MajorOperatingSystemVersion { get; set; }
        public ushort MinorOperatingSystemVersion { get; set; }
        public ushort MajorImageVersion { get; set; }
        public ushort MinorImageVersion { get; set; }
        public ushort MajorSubsystemVersion { get; set; }
        public ushort MinorSubsystemVersion { get; set; }
        public uint Win32VersionValue { get; set; }
        public uint SizeOfImage { get; set; }
        public uint SizeOfHeaders { get; set; }
        public uint CheckSum { get; set; }
        public ushort Subsystem { get; set; }
        public ushort DllCharacteristics { get; set; }
        public ulong SizeOfStackReserve { get; set; }
        public ulong SizeOfStackCommit { get; set; }
        public ulong SizeOfHeapReserve { get; set; }
        public ulong SizeOfHeapCommit { get; set; }
        public uint LoaderFlags { get; set; }
        public uint NumberOfRvaAndSizes { get; set; }

        public DataDirectory[] DataDirectories { get; set; } = Array.Empty<DataDirectory>();

        public bool Is64Bit => Magic == MagicPe32Plus;
        public bool Is32Bit => Magic == MagicPe32;

        public static OptionalHeader Read(BinaryReader reader, ushort sizeOfOptionalHeader)
        {
            var startPos = reader.BaseStream.Position;
            var header = new OptionalHeader();

            header.Magic = reader.ReadUInt16();
            bool is64 = header.Magic == MagicPe32Plus;

            header.MajorLinkerVersion = reader.ReadByte();
            header.MinorLinkerVersion = reader.ReadByte();
            header.SizeOfCode = reader.ReadUInt32();
            header.SizeOfInitializedData = reader.ReadUInt32();
            header.SizeOfUninitializedData = reader.ReadUInt32();
            header.AddressOfEntryPoint = reader.ReadUInt32();
            header.BaseOfCode = reader.ReadUInt32();

            if (!is64)
                header.BaseOfData = reader.ReadUInt32();

            header.ImageBase = is64 ? reader.ReadUInt64() : reader.ReadUInt32();
            header.SectionAlignment = reader.ReadUInt32();
            header.FileAlignment = reader.ReadUInt32();
            header.MajorOperatingSystemVersion = reader.ReadUInt16();
            header.MinorOperatingSystemVersion = reader.ReadUInt16();
            header.MajorImageVersion = reader.ReadUInt16();
            header.MinorImageVersion = reader.ReadUInt16();
            header.MajorSubsystemVersion = reader.ReadUInt16();
            header.MinorSubsystemVersion = reader.ReadUInt16();
            header.Win32VersionValue = reader.ReadUInt32();
            header.SizeOfImage = reader.ReadUInt32();
            header.SizeOfHeaders = reader.ReadUInt32();
            header.CheckSum = reader.ReadUInt32();
            header.Subsystem = reader.ReadUInt16();
            header.DllCharacteristics = reader.ReadUInt16();
            header.SizeOfStackReserve = is64 ? reader.ReadUInt64() : reader.ReadUInt32();
            header.SizeOfStackCommit = is64 ? reader.ReadUInt64() : reader.ReadUInt32();
            header.SizeOfHeapReserve = is64 ? reader.ReadUInt64() : reader.ReadUInt32();
            header.SizeOfHeapCommit = is64 ? reader.ReadUInt64() : reader.ReadUInt32();
            header.LoaderFlags = reader.ReadUInt32();
            header.NumberOfRvaAndSizes = reader.ReadUInt32();

            uint count = header.NumberOfRvaAndSizes;
            if (count > 16)
                count = 16;

            header.DataDirectories = new DataDirectory[count];
            for (uint i = 0; i < count; i++)
            {
                header.DataDirectories[i] = DataDirectory.Read(reader);
            }

            long consumed = reader.BaseStream.Position - startPos;
            if (consumed < sizeOfOptionalHeader)
            {
                reader.BaseStream.Seek(startPos + sizeOfOptionalHeader, SeekOrigin.Begin);
            }

            return header;
        }

        public DataDirectory? GetDirectory(int index)
        {
            if (DataDirectories == null || index < 0 || index >= DataDirectories.Length)
                return null;
            return DataDirectories[index];
        }

        public override string ToString() =>
            $"OptionalHeader {(Is64Bit ? "PE32+" : "PE32")} EntryPoint=0x{AddressOfEntryPoint:X} ImageBase=0x{ImageBase:X}";
    }
}