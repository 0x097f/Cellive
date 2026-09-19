using System.IO;

namespace CilDotNet.Executable
{
    public sealed class SectionHeader
    {
        public string Name { get; set; } = string.Empty;
        public uint VirtualSize { get; set; }
        public uint VirtualAddress { get; set; }
        public uint SizeOfRawData { get; set; }
        public uint PointerToRawData { get; set; }
        public uint PointerToRelocations { get; set; }
        public uint PointerToLinenumbers { get; set; }
        public ushort NumberOfRelocations { get; set; }
        public ushort NumberOfLinenumbers { get; set; }
        public uint Characteristics { get; set; }

        public static SectionHeader Read(BinaryReader reader)
        {
            var header = new SectionHeader();

            var nameBytes = reader.ReadBytes(8);
            int nameLen = 0;
            while (nameLen < 8 && nameBytes[nameLen] != 0)
                nameLen++;
            header.Name = System.Text.Encoding.ASCII.GetString(nameBytes, 0, nameLen);

            header.VirtualSize = reader.ReadUInt32();
            header.VirtualAddress = reader.ReadUInt32();
            header.SizeOfRawData = reader.ReadUInt32();
            header.PointerToRawData = reader.ReadUInt32();
            header.PointerToRelocations = reader.ReadUInt32();
            header.PointerToLinenumbers = reader.ReadUInt32();
            header.NumberOfRelocations = reader.ReadUInt16();
            header.NumberOfLinenumbers = reader.ReadUInt16();
            header.Characteristics = reader.ReadUInt32();

            return header;
        }

        public bool HasFlag(uint flag) => (Characteristics & flag) != 0;

        public bool IsExecutable => HasFlag(0x20000000);
        public bool IsReadable => HasFlag(0x40000000);
        public bool IsWritable => HasFlag(0x80000000);

        public override string ToString() => $"{Name} RVA=0x{VirtualAddress:X} Size=0x{SizeOfRawData:X}";
    }
}