namespace CilDotNet.Executable
{
    public sealed class PeImage
    {
        private readonly byte[] _rawData;

        public DosHeader DosHeader { get; }
        public NtHeaders NtHeaders { get; }
        public CliHeader CliHeader { get; }
        public IReadOnlyList<SectionHeader> SectionHeaders { get; }
        public IReadOnlyList<SectionData> Sections { get; }

        public bool Managed => CliHeader != null;
        public bool Is64Bit => NtHeaders.Is64Bit;
        public bool IsDll => NtHeaders.FileHeader.IsDll;
        public Architectures Architecture => NtHeaders.FileHeader.Architecture;
        public uint EntryPointRva => NtHeaders.OptionalHeader.AddressOfEntryPoint;
        public ulong ImageBase => NtHeaders.OptionalHeader.ImageBase;

        private PeImage(byte[] data)
        {
            _rawData = data;

            DosHeader = DosHeader.Read(data);
            if (!DosHeader.IsValid)
                throw new InvalidDataException("Invalid DOS header");

            using var ms = new MemoryStream(data);
            using var reader = new BinaryReader(ms);

            NtHeaders = NtHeaders.Read(reader, DosHeader.PeHeaderOffset);
            if (!NtHeaders.IsValid)
                throw new InvalidDataException("Invalid NT headers");

            SectionHeaders = ReadSectionHeaders(reader);
            Sections = ReadSectionData(SectionHeaders);
            Rva.SetSections(Sections.Select(ConvertToSectionInfo).ToList());

            CliHeader = ReadCliHeader();
        }

        public static PeImage Load(string path) => Load(File.ReadAllBytes(path));
        public static PeImage Load(byte[] data) => new PeImage(data);
        public static PeImage Load(Stream stream)
        {
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return new PeImage(ms.ToArray());
        }

        public byte[] ReadRva(uint rva, int size)
        {
            foreach (var section in Sections)
            {
                if (rva >= section.VirtualAddress &&
                    rva < section.VirtualAddress + Math.Max(section.VirtualSize, section.SizeOfRawData))
                {
                    return section.ReadBytes(rva, size);
                }
            }
            return Array.Empty<byte>();
        }

        public byte[] GetSectionData(string name)
        {
            var section = Sections.FirstOrDefault(s =>
                string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));
            return section?.Data ?? Array.Empty<byte>();
        }

        public SectionData? GetSection(uint rva)
        {
            foreach (var section in Sections)
            {
                if (rva >= section.VirtualAddress &&
                    rva < section.VirtualAddress + Math.Max(section.VirtualSize, section.SizeOfRawData))
                {
                    return section;
                }
            }
            return null;
        }

        public bool IsPe32 => NtHeaders.OptionalHeader.Is32Bit;
        public bool IsPe32Plus => NtHeaders.OptionalHeader.Is64Bit;

        public bool IsCorFlagsSet(CorFlags flag)
            => (CliHeader.Flags & (uint)flag) != 0;

        public bool IsIlOnly => IsCorFlagsSet(CorFlags.COMIMAGE_FLAGS_ILONLY);
        public bool Is32BitRequired => IsCorFlagsSet(CorFlags.COMIMAGE_FLAGS_32BITREQUIRED);
        public bool Is32BitPreferred => IsCorFlagsSet(CorFlags.COMIMAGE_FLAGS_32BITPREFERRED);
        public bool IsStrongNameSigned => IsCorFlagsSet(CorFlags.COMIMAGE_FLAGS_STRONGNAMESIGNED);
        public bool HasNativeEntryPoint => IsCorFlagsSet(CorFlags.COMIMAGE_FLAGS_NATIVE_ENTRYPOINT);

        public DataDirectory? GetDirectory(int index)
            => NtHeaders.OptionalHeader.GetDirectory(index);

        public DataDirectory MetadataDirectory => CliHeader?.MetaData ?? default;
        public DataDirectory ResourcesDirectory => CliHeader?.Resources ?? default;
        public DataDirectory StrongNameDirectory => CliHeader?.StrongNameSignature ?? default;

        private IReadOnlyList<SectionHeader> ReadSectionHeaders(BinaryReader reader)
        {
            reader.BaseStream.Seek(
                DosHeader.PeHeaderOffset + 4 + 20 + NtHeaders.FileHeader.SizeOfOptionalHeader,
                SeekOrigin.Begin);

            var headers = new List<SectionHeader>();
            for (int i = 0; i < NtHeaders.FileHeader.NumberOfSections; i++)
            {
                headers.Add(SectionHeader.Read(reader));
            }
            return headers;
        }

        private IReadOnlyList<SectionData> ReadSectionData(IReadOnlyList<SectionHeader> headers)
        {
            var sections = new List<SectionData>();
            foreach (var h in headers)
            {
                var data = new byte[h.SizeOfRawData];
                int offset = (int)h.PointerToRawData;
                if (offset >= 0 && offset + data.Length <= _rawData.Length)
                    Array.Copy(_rawData, offset, data, 0, data.Length);

                sections.Add(new SectionData
                {
                    Name = h.Name,
                    Data = data,
                    VirtualAddress = h.VirtualAddress,
                    VirtualSize = h.VirtualSize,
                    PointerToRawData = h.PointerToRawData,
                    SizeOfRawData = h.SizeOfRawData,
                });
            }
            return sections;
        }

        private CliHeader? ReadCliHeader()
        {
            var cliDir = GetDirectory(DirectoryIndex.ClrHeader);
            if (cliDir == null || cliDir.Value.IsEmpty)
                return null;

            var data = ReadRva(cliDir.Value.VirtualAddress, (int)cliDir.Value.Size);
            if (data.Length < 72)
                return null;

            using var ms = new MemoryStream(data);
            using var reader = new BinaryReader(ms);
            return CliHeader.Read(reader);
        }

        private static SectionInfo ConvertToSectionInfo(SectionData data)
        {
            return new SectionInfo
            {
                Name = data.Name,
                VirtualSize = data.VirtualSize,
                VirtualAddress = data.VirtualAddress,
                SizeOfRawData = data.SizeOfRawData,
                PointerToRawData = data.PointerToRawData,
            };
        }

        public override string ToString() =>
            $"PE {Architecture.FriendlyName()} {(IsDll ? "DLL" : "EXE")} " +
            $"{(Managed ? "Managed" : "Native")} {(Is64Bit ? "64-bit" : "32-bit")}";
    }
}