namespace Cellive.Cil.Metadata.Tables
{
    public sealed class DeclSecurityTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<DeclSecurityRow> rows = new();

        public IReadOnlyList<DeclSecurityRow> Rows => rows;
        public int Count => rows.Count;

        public DeclSecurityTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.DeclSecurity);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.DeclSecurity);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new DeclSecurityRow
                {
                    Action = reader.ReadUInt16(),
                    Parent = reader.ReadCodedIndex(CodedIndex.HasDeclSecurity).Value,
                    PermissionSetIndex = reader.ReadBlobIndex(),
                };

                row.ParentToken = new MetadataToken(row.Parent);
                row.PermissionSet = metadata.Blobs.Get(row.PermissionSetIndex);

                rows.Add(row);
            }
        }

        public DeclSecurityRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public IEnumerable<DeclSecurityRow> FindByParent(MetadataToken parent)
        {
            foreach (var row in rows)
            {
                if (row.ParentToken == parent)
                    yield return row;
            }
        }
    }
}