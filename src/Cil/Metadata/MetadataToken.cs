namespace CilDotNet.Cil.Metadata
{
    public readonly struct MetadataToken : IEquatable<MetadataToken>
    {
        public uint Value { get; }

        public MetadataToken(uint value) => Value = value;

        public byte TableIndex => (byte)(Value >> 24);
        public uint RowIndex => Value & 0x00FFFFFF;

        public TableType Table => (TableType)TableIndex;

        public bool IsNil => Value == 0;

        public static MetadataToken FromToken(TableType table, uint row)
            => new MetadataToken(((uint)table << 24) | (row & 0x00FFFFFF));

        public static bool operator ==(MetadataToken left, MetadataToken right) => left.Value == right.Value;
        public static bool operator !=(MetadataToken left, MetadataToken right) => left.Value != right.Value;

        public bool Equals(MetadataToken other) => Value == other.Value;
        public override bool Equals(object? obj) => obj is MetadataToken other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => $"0x{Value:X8} ({Table}[{RowIndex}])";
    }
}