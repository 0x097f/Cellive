using System.IO;

namespace CilDotNet.Executable
{
    public struct DataDirectory
    {
        public uint VirtualAddress { get; set; }
        public uint Size { get; set; }

        public bool IsEmpty => VirtualAddress == 0 && Size == 0;

        public static DataDirectory Read(BinaryReader reader)
        {
            return new DataDirectory
            {
                VirtualAddress = reader.ReadUInt32(),
                Size = reader.ReadUInt32(),
            };
        }

        public override string ToString() =>
            IsEmpty ? "(empty)" : $"RVA=0x{VirtualAddress:X8} Size=0x{Size:X8}";
    }
}