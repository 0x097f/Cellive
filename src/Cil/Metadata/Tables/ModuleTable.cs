namespace Cellive.Cil.Metadata.Tables
{
    public sealed class ModuleTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<ModuleRow> rows = new();

        public IReadOnlyList<ModuleRow> Rows => rows;
        public int Count => rows.Count;

        public ModuleTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.Module);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.Module);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new ModuleRow
                {
                    Generation = reader.ReadUInt16(),
                    NameIndex = reader.ReadStringIndex(),
                    MvidIndex = reader.ReadGuidIndex(),
                    EncIdIndex = reader.ReadGuidIndex(),
                    EncBaseIdIndex = reader.ReadGuidIndex(),
                };

                row.Name = metadata.Strings.Get(row.NameIndex);
                row.Mvid = metadata.Guids.Get(row.MvidIndex);
                row.EncId = metadata.Guids.Get(row.EncIdIndex);
                row.EncBaseId = metadata.Guids.Get(row.EncBaseIdIndex);

                rows.Add(row);
            }
        }

        public ModuleRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public ModuleRow? First => rows.Count > 0 ? rows[0] : null;
    }
}