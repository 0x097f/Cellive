namespace CilDotNet.Executable
{
    public sealed class SectionData
    {
        public string Name { get; set; } = string.Empty;
        public byte[] Data { get; set; } = System.Array.Empty<byte>();

        public uint VirtualAddress { get; set; }
        public uint VirtualSize { get; set; }
        public uint PointerToRawData { get; set; }
        public uint SizeOfRawData { get; set; }

        public int Length => Data.Length;

        public byte[] ReadBytes(uint rva, int count)
        {
            if (rva < VirtualAddress)
                return System.Array.Empty<byte>();

            var offset = (int)(rva - VirtualAddress);
            if (offset < 0 || offset >= Data.Length)
                return System.Array.Empty<byte>();

            var available = Data.Length - offset;
            var size = count < available ? count : available;

            var result = new byte[size];
            System.Array.Copy(Data, offset, result, 0, size);
            return result;
        }

        public override string ToString() => $"{Name} ({Data.Length} bytes)";
    }
}