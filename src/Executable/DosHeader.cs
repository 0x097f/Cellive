namespace Cellive.Executable
{
    public sealed class DosHeader
    {
        public const ushort ExpectedMagic = 0x5A4D; //MZ

        public ushort Magic { get; set; }
        public ushort BytesOnLastPage { get; set; }
        public ushort PagesInFile { get; set; }
        public ushort Relocations { get; set; }
        public ushort SizeOfHeader { get; set; }
        public ushort MinExtraParagraphs { get; set; }
        public ushort MaxExtraParagraphs { get; set; }
        public ushort InitialSs { get; set; }
        public ushort InitialSp { get; set; }
        public ushort Checksum { get; set; }
        public ushort InitialIp { get; set; }
        public ushort InitialCs { get; set; }
        public ushort RelocationTableOffset { get; set; }
        public ushort OverlayNumber { get; set; }
        public ushort[] Reserved { get; set; } = new ushort[4];
        public ushort OemId { get; set; }
        public ushort OemInfo { get; set; }
        public ushort[] Reserved2 { get; set; } = new ushort[10];
        public int PeHeaderOffset { get; set; }

        public bool IsValid => Magic == ExpectedMagic;

        public static DosHeader Read(BinaryReader reader)
        {
            var header = new DosHeader
            {
                Magic = reader.ReadUInt16(),
                BytesOnLastPage = reader.ReadUInt16(),
                PagesInFile = reader.ReadUInt16(),
                Relocations = reader.ReadUInt16(),
                SizeOfHeader = reader.ReadUInt16(),
                MinExtraParagraphs = reader.ReadUInt16(),
                MaxExtraParagraphs = reader.ReadUInt16(),
                InitialSs = reader.ReadUInt16(),
                InitialSp = reader.ReadUInt16(),
                Checksum = reader.ReadUInt16(),
                InitialIp = reader.ReadUInt16(),
                InitialCs = reader.ReadUInt16(),
                RelocationTableOffset = reader.ReadUInt16(),
                OverlayNumber = reader.ReadUInt16(),
            };

            for (int i = 0; i < 4; i++)
                header.Reserved[i] = reader.ReadUInt16();

            header.OemId = reader.ReadUInt16();
            header.OemInfo = reader.ReadUInt16();

            for (int i = 0; i < 10; i++)
                header.Reserved2[i] = reader.ReadUInt16();

            header.PeHeaderOffset = reader.ReadInt32();

            return header;
        }

        public static DosHeader Read(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var reader = new BinaryReader(ms);
            return Read(reader);
        }

        public override string ToString() => $"DOS Header (MZ={IsValid}, PE@0x{PeHeaderOffset:X})";
    }
}