namespace Cellive.Cil.Metadata.Tables
{
    [Obsolete("Obsolete since .NET 2.0, only present in ENC or very old assemblies")]
    public sealed class FieldPtrTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<MetadataToken> rows = new();

        public IReadOnlyList<MetadataToken> Rows => rows;
        public int Count => rows.Count;

        public FieldPtrTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.FieldPtr);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.FieldPtr);

            for (int i = 0; i < info.RowCount; i++)
            {
                var fieldIndex = reader.ReadTableIndex(TableType.Field);
                rows.Add(MetadataToken.FromToken(TableType.Field, fieldIndex));
            }
        }

        public MetadataToken? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }
    }

    [Obsolete("Obsolete since .NET 2.0, only present in ENC or very old assemblies")]
    public sealed class MethodPtrTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<MetadataToken> rows = new();

        public IReadOnlyList<MetadataToken> Rows => rows;
        public int Count => rows.Count;

        public MethodPtrTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.MethodPtr);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.MethodPtr);

            for (int i = 0; i < info.RowCount; i++)
            {
                var methodIndex = reader.ReadTableIndex(TableType.Method);
                rows.Add(MetadataToken.FromToken(TableType.Method, methodIndex));
            }
        }

        public MetadataToken? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }
    }

    [Obsolete("Obsolete since .NET 2.0, only present in ENC or very old assemblies")]
    public sealed class ParamPtrTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<MetadataToken> rows = new();

        public IReadOnlyList<MetadataToken> Rows => rows;
        public int Count => rows.Count;

        public ParamPtrTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.ParamPtr);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.ParamPtr);

            for (int i = 0; i < info.RowCount; i++)
            {
                var paramIndex = reader.ReadTableIndex(TableType.Param);
                rows.Add(MetadataToken.FromToken(TableType.Param, paramIndex));
            }
        }

        public MetadataToken? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }
    }

    [Obsolete("Obsolete since .NET 2.0, only present in ENC or very old assemblies")]
    public sealed class EventPtrTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<MetadataToken> rows = new();

        public IReadOnlyList<MetadataToken> Rows => rows;
        public int Count => rows.Count;

        public EventPtrTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.EventPtr);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.EventPtr);

            for (int i = 0; i < info.RowCount; i++)
            {
                var eventIndex = reader.ReadTableIndex(TableType.Event);
                rows.Add(MetadataToken.FromToken(TableType.Event, eventIndex));
            }
        }

        public MetadataToken? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }
    }

    [Obsolete("Obsolete since .NET 2.0, only present in ENC or very old assemblies")]
    public sealed class PropertyPtrTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<MetadataToken> rows = new();

        public IReadOnlyList<MetadataToken> Rows => rows;
        public int Count => rows.Count;

        public PropertyPtrTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.PropertyPtr);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.PropertyPtr);

            for (int i = 0; i < info.RowCount; i++)
            {
                var propertyIndex = reader.ReadTableIndex(TableType.Property);
                rows.Add(MetadataToken.FromToken(TableType.Property, propertyIndex));
            }
        }

        public MetadataToken? GetRow(int index)
        {
            if (index < 0 || index >= rows.Count)
                return null;
            return rows[index];
        }
    }

    [Obsolete("Obsolete since .NET 1.1, do not use")]
    public sealed class AssemblyOSTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<(uint OSPlatformID, uint OSMajorVersion, uint OSMinorVersion)> rows = new();

        public IReadOnlyList<(uint OSPlatformID, uint OSMajorVersion, uint OSMinorVersion)> Rows => rows;
        public int Count => rows.Count;

        public AssemblyOSTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.AssemblyOS);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.AssemblyOS);

            for (int i = 0; i < info.RowCount; i++)
            {
                var platformId = reader.ReadUInt32();
                var major = reader.ReadUInt32();
                var minor = reader.ReadUInt32();
                rows.Add((platformId, major, minor));
            }
        }
    }

    [Obsolete("Obsolete since .NET 1.1, do not use")]
    public sealed class AssemblyProcessorTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<uint> rows = new();

        public IReadOnlyList<uint> Rows => rows;
        public int Count => rows.Count;

        public AssemblyProcessorTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.AssemblyProcessor);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.AssemblyProcessor);

            for (int i = 0; i < info.RowCount; i++)
            {
                rows.Add(reader.ReadUInt32());
            }
        }
    }

    [Obsolete("Obsolete since .NET 1.1, do not use")]
    public sealed class AssemblyRefOSTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<(uint OSPlatformID, uint OSMajorVersion, uint OSMinorVersion, uint AssemblyRefIndex)> rows = new();

        public IReadOnlyList<(uint OSPlatformID, uint OSMajorVersion, uint OSMinorVersion, uint AssemblyRefIndex)> Rows => rows;
        public int Count => rows.Count;

        public AssemblyRefOSTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.AssemblyRefOS);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.AssemblyRefOS);

            for (int i = 0; i < info.RowCount; i++)
            {
                var platformId = reader.ReadUInt32();
                var major = reader.ReadUInt32();
                var minor = reader.ReadUInt32();
                var assemblyRef = reader.ReadTableIndex(TableType.AssemblyRef);
                rows.Add((platformId, major, minor, assemblyRef));
            }
        }
    }

    [Obsolete("Obsolete since .NET 1.1, do not use")]
    public sealed class AssemblyRefProcessorTable
    {
        private readonly MetadataReader metadata;
        private readonly TableReader reader;
        private readonly List<(uint Processor, uint AssemblyRefIndex)> rows = new();

        public IReadOnlyList<(uint Processor, uint AssemblyRefIndex)> Rows => rows;
        public int Count => rows.Count;

        public AssemblyRefProcessorTable(MetadataReader metadata, TableDecoder decoder, BinaryReader binaryReader)
        {
            this.metadata = metadata;
            reader = new TableReader(metadata, decoder, binaryReader);
            Read();
        }

        private void Read()
        {
            var info = metadata.GetTableInfo(TableType.AssemblyRefProcessor);
            if (!info.HasRows)
                return;

            reader.SeekToTableStart(TableType.AssemblyRefProcessor);

            for (int i = 0; i < info.RowCount; i++)
            {
                var processor = reader.ReadUInt32();
                var assemblyRef = reader.ReadTableIndex(TableType.AssemblyRef);
                rows.Add((processor, assemblyRef));
            }
        }
    }
}
