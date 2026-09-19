namespace CilDotNet.Cil.Metadata.Tables
{
    public static class CodedIndexReader
    {
        public static MetadataToken Decode(CodedIndex index, uint value)
        {
            var tables = CodedIndexTables.Tables[(int)index];
            var tagBits = CodedIndexTables.TagBits[(int)index];
            var tag = (int)(value & ((1u << tagBits) - 1));
            var row = value >> tagBits;

            if (tag >= tables.Length)
                return default;

            return MetadataToken.FromToken(tables[tag], row);
        }

        public static uint Encode(CodedIndex index, MetadataToken token)
        {
            var tables = CodedIndexTables.Tables[(int)index];
            var tagBits = CodedIndexTables.TagBits[(int)index];

            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i] == token.Table)
                    return (token.RowIndex << tagBits) | (uint)i;
            }

            return 0;
        }

        public static int GetSize(CodedIndex index, System.Collections.Generic.IReadOnlyDictionary<TableType, TableInfo> tables)
        {
            var tagBits = CodedIndexTables.TagBits[(int)index];
            var candidates = CodedIndexTables.Tables[(int)index];

            uint maxRows = 0;
            foreach (var table in candidates)
            {
                if (table == TableType.NotUsed)
                    continue;

                if (tables.TryGetValue(table, out var info))
                {
                    if ((uint)info.RowCount > maxRows)
                        maxRows = (uint)info.RowCount;
                }
            }
            return (maxRows << tagBits) < (1 << 16) ? 2 : 4;
        }
    }
}