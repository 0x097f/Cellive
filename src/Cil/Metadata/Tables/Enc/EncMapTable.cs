namespace CilDotNet.Cil.Metadata.Tables.Enc
{
    public sealed class EncMapTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<EncMapRow> rows = new();

        public IReadOnlyList<EncMapRow> Rows => rows;
        public int Count => rows.Count;

        public EncMapTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.ENCMap);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.ENCMap);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new EncMapRow
                {
                    Token = reader.ReadUInt32(),
                };

                row.TokenValue = new MetadataToken(row.Token);
                rows.Add(row);
            }
        }

        public EncMapRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }
    }
}