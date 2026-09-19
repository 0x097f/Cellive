namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class InterfaceImplTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<InterfaceImplRow> rows = new();

        public IReadOnlyList<InterfaceImplRow> Rows => rows;
        public int Count => rows.Count;

        public InterfaceImplTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.InterfaceImpl);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.InterfaceImpl);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new InterfaceImplRow
                {
                    Class = reader.ReadTableIndex(TableType.TypeDef),
                    Interface = reader.ReadCodedIndex(CodedIndex.TypeDefOrRef).Value,
                };

                row.ClassToken = MetadataToken.FromToken(TableType.TypeDef, row.Class);
                row.InterfaceToken = new MetadataToken(row.Interface);

                rows.Add(row);
            }
        }

        public InterfaceImplRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public IEnumerable<InterfaceImplRow> FindByClass(MetadataToken classToken)
        {
            foreach (var row in rows)
            {
                if (row.ClassToken == classToken)
                    yield return row;
            }
        }
    }
}