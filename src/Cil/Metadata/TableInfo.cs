namespace CilDotNet.Cil.Metadata
{
    public struct TableInfo
    {
        public TableType Type { get; set; }
        public int RowCount { get; set; }
        public int RowSize { get; set; }
        public long Offset { get; set; }

        public bool HasRows => RowCount > 0;

        public override string ToString() => $"{Type} ({RowCount} rows) @ 0x{Offset:X}";
    }
}