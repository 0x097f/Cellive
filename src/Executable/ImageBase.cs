namespace Cellive.Executable
{
    public sealed class ImageBase
    {
        public uint Value { get; set; }
        public bool Is64Bit { get; set; }

        public ImageBase(uint value, bool is64Bit)
        {
            Value = value;
            Is64Bit = is64Bit;
        }

        public override string ToString() => $"0x{Value:X}";
    }
}