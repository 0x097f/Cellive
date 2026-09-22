namespace Cellive.Cil.Metadata.Tables
{
    public sealed class MethodImplTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<MethodImplRow> rows = new();

        public IReadOnlyList<MethodImplRow> Rows => rows;
        public int Count => rows.Count;

        public MethodImplTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.MethodImpl);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.MethodImpl);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new MethodImplRow
                {
                    Class = reader.ReadTableIndex(TableType.TypeDef),
                    MethodBody = reader.ReadCodedIndex(CodedIndex.MethodDefOrRef).Value,
                    MethodDeclaration = reader.ReadCodedIndex(CodedIndex.MethodDefOrRef).Value,
                };

                row.ClassToken = MetadataToken.FromToken(TableType.TypeDef, row.Class);
                row.MethodBodyToken = new MetadataToken(row.MethodBody);
                row.MethodDeclarationToken = new MetadataToken(row.MethodDeclaration);

                rows.Add(row);
            }
        }

        public MethodImplRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public IEnumerable<MethodImplRow> FindByClass(MetadataToken classToken)
        {
            foreach (var row in rows)
            {
                if (row.ClassToken == classToken)
                    yield return row;
            }
        }
    }
}