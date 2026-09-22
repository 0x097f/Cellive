namespace Cellive.Cil.Metadata.Tables
{
    public sealed class ModuleRefTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<ModuleRefRow> rows = new();

        public IReadOnlyList<ModuleRefRow> Rows => rows;
        public int Count => rows.Count;

        public ModuleRefTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.ModuleRef);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.ModuleRef);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new ModuleRefRow
                {
                    NameIndex = reader.ReadStringIndex(),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                rows.Add(row);
            }
        }

        public ModuleRefRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public ModuleRefRow? FindByName(string name)
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