namespace Cellive.Cil.Metadata.Tables
{
    public sealed class ConstantRow
    {
        public byte Type { get; set; }
        public byte Padding { get; set; }
        public uint Parent { get; set; }
        public uint ValueIndex { get; set; }

        public byte[] Value { get; set; } = System.Array.Empty<byte>();
        public MetadataToken ParentToken { get; set; }

        public ElementTypeKind ElementType => (ElementTypeKind)Type;

        public override string ToString() =>
            $"Constant {ElementType} -> {ParentToken} ({Value.Length} bytes)";
    }

    public enum ElementTypeKind : byte
    {
        End = 0x00,
        Void = 0x01,
        Boolean = 0x02,
        Char = 0x03,
        I1 = 0x04,
        U1 = 0x05,
        I2 = 0x06,
        U2 = 0x07,
        I4 = 0x08,
        U4 = 0x09,
        I8 = 0x0A,
        U8 = 0x0B,
        R4 = 0x0C,
        R8 = 0x0D,
        String = 0x0E,
        Class = 0x12,
    }
}