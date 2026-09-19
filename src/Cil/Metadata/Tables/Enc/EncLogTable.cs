namespace CilDotNet.Cil.Metadata.Tables.Enc
{
    public sealed class EncLogTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<EncLogRow> rows = new();

        public IReadOnlyList<EncLogRow> Rows => rows;
        public int Count => rows.Count;

        public EncLogTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.ENCLog);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.ENCLog);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new EncLogRow
                {
                    Token = reader.ReadUInt32(),
                    FuncCode = reader.ReadUInt32(),
                };

                row.TokenValue = new MetadataToken(row.Token);
                rows.Add(row);
            }
        }

        public EncLogRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }
    }
}