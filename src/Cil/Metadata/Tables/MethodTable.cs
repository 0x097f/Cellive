namespace Cellive.Cil.Metadata.Tables
{
    public sealed class MethodTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<MethodRow> rows = new();

        public IReadOnlyList<MethodRow> Rows => rows;
        public int Count => rows.Count;

        public MethodTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.Method);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.Method);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new MethodRow
                {
                    RVA = reader.ReadUInt32(),
                    ImplFlags = reader.ReadUInt16(),
                    Flags = reader.ReadUInt16(),
                    NameIndex = reader.ReadStringIndex(),
                    SignatureIndex = reader.ReadBlobIndex(),
                    ParamList = reader.ReadTableIndex(TableType.Param),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.Signature = metadata.Blobs.Get(row.SignatureIndex);
                row.ParamListToken = MetadataToken.FromToken(TableType.Param, row.ParamList);

                rows.Add(row);
            }
        }

        public MethodRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public MethodRow? FindByName(string name)
        {
            foreach (var row in rows)
            {
                if (row.Name == name)
                    return row;
            }
            return null;
        }
    }
}