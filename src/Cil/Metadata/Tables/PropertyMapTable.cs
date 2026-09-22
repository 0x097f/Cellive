using System.Collections.Generic;
using System.IO;

namespace Cellive.Cil.Metadata.Tables
{
    public sealed class PropertyMapTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<PropertyMapRow> rows = new();

        public IReadOnlyList<PropertyMapRow> Rows => rows;
        public int Count => rows.Count;

        public PropertyMapTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.PropertyMap);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.PropertyMap);

            for (int i = 0; i < info.RowCount; i++)
            {
                var row = new PropertyMapRow
                {
                    Parent = reader.ReadTableIndex(TableType.TypeDef),
                    PropertyList = reader.ReadTableIndex(TableType.Property),
                };

                row.ParentToken = MetadataToken.FromToken(TableType.TypeDef, row.Parent);
                row.PropertyListToken = MetadataToken.FromToken(TableType.Property, row.PropertyList);

                rows.Add(row);
            }
        }

        public PropertyMapRow? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }

        public PropertyMapRow? FindByParent(MetadataToken parent)
        {
            foreach (var row in rows)
            {
                if (row.ParentToken == parent)
                    return row;
            }
            return null;
        }
    }
}