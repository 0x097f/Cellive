namespace Cellive.Cil.Metadata.Tables
{
    public sealed class ImplMapTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<ImplMapRow> rows = new();

        public IReadOnlyList<ImplMapRow> Rows => rows;
        public int Count => rows.Count;

        public ImplMapTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.ImplMap);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.ImplMap);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new ImplMapRow
                {
                    MappingFlags = reader.ReadUInt16(),
                    MemberForwarded = reader.ReadCodedIndex(CodedIndex.MemberForwarded).Value,
                    ImportNameIndex = reader.ReadStringIndex(),
                    ImportScope = reader.ReadTableIndex(TableType.ModuleRef),
                };

                row.ImportName = metadata.Strings.Get(row.ImportNameIndex);
                row.MemberForwardedToken = new MetadataToken(row.MemberForwarded);
                row.ImportScopeToken = MetadataToken.FromToken(TableType.ModuleRef, row.ImportScope);

                rows.Add(row);
            }
        }

        public ImplMapRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public ImplMapRow? FindByMember(MetadataToken member)
        {
            foreach (var row in rows)
            {
                if (row.MemberForwardedToken == member)
                    return row;
            }
            return null;
        }
    }
}