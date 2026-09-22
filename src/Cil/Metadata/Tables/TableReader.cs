namespace Cellive.Cil.Metadata.Tables
{
    public sealed class TableReader
    {
        private readonly MetadataReader metadata;
        private readonly TableDecoder decoder;
        private readonly BinaryReader reader;

        public TableReader(MetadataReader metadata, TableDecoder decoder, BinaryReader reader)
        {
            this.metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            this.decoder = decoder ?? throw new ArgumentNullException(nameof(decoder));
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public MetadataReader Metadata => metadata;
        public TableDecoder Decoder => decoder;
        public BinaryReader Reader => reader;

        public void Seek(TableType table, int row)
        {
            var info = metadata.GetTableInfo(table);
            if (!info.HasRows)
                throw new InvalidOperationException($"Table {table} has no rows");

            if (row < 0 || row >= info.RowCount)
                throw new ArgumentOutOfRangeException(nameof(row));

            var rowSize = decoder.GetRowSize(table);
            var offset = info.Offset + (long)row * rowSize;
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        }

        public void SeekToTableStart(TableType table)
        {
            var info = metadata.GetTableInfo(table);
            if (!info.HasRows)
                throw new InvalidOperationException($"Table {table} has no rows");

            reader.BaseStream.Seek(info.Offset, SeekOrigin.Begin);
        }

        public byte ReadByte() => reader.ReadByte();

        public ushort ReadUInt16() => reader.ReadUInt16();

        public uint ReadUInt32() => reader.ReadUInt32();

        public ulong ReadUInt64() => reader.ReadUInt64();

        public short ReadInt16() => reader.ReadInt16();

        public int ReadInt32() => reader.ReadInt32();

        public long ReadInt64() => reader.ReadInt64();

        public uint ReadStringIndex()
        {
            return metadata.IsStringHeapLarge ? reader.ReadUInt32() : reader.ReadUInt16();
        }

        public uint ReadBlobIndex()
        {
            return metadata.IsBlobHeapLarge ? reader.ReadUInt32() : reader.ReadUInt16();
        }

        public uint ReadGuidIndex()
        {
            return metadata.IsGuidHeapLarge ? reader.ReadUInt32() : reader.ReadUInt16();
        }

        public string ReadString()
        {
            var index = ReadStringIndex();
            return metadata.Strings.Get(index);
        }

        public byte[] ReadBlob()
        {
            var index = ReadBlobIndex();
            return metadata.Blobs.Get(index);
        }

        public Guid ReadGuid()
        {
            var index = ReadGuidIndex();
            return metadata.Guids.Get(index);
        }

        public string ReadUserString()
        {
            var index = metadata.IsBlobHeapLarge ? reader.ReadUInt32() : reader.ReadUInt16();
            return metadata.UserStrings.Get(index);
        }

        public uint ReadTableIndex(TableType targetTable)
        {
            var rows = metadata.GetRowCount(targetTable);
            return rows > 0xFFFF ? reader.ReadUInt32() : reader.ReadUInt16();
        }

        public MetadataToken ReadTableToken(TableType targetTable)
        {
            var row = ReadTableIndex(targetTable);
            return MetadataToken.FromToken(targetTable, row);
        }

        public MetadataToken ReadCodedIndex(CodedIndex index)
        {
            var size = CodedIndexReader.GetSize(index, metadata.Tables);
            var value = size == 4 ? reader.ReadUInt32() : reader.ReadUInt16();
            return CodedIndexReader.Decode(index, value);
        }

        public MetadataToken ReadTypeDefOrRef() => ReadCodedIndex(CodedIndex.TypeDefOrRef);
        public MetadataToken ReadHasConstant() => ReadCodedIndex(CodedIndex.HasConstant);
        public MetadataToken ReadHasCustomAttribute() => ReadCodedIndex(CodedIndex.HasCustomAttribute);
        public MetadataToken ReadHasFieldMarshal() => ReadCodedIndex(CodedIndex.HasFieldMarshal);
        public MetadataToken ReadHasDeclSecurity() => ReadCodedIndex(CodedIndex.HasDeclSecurity);
        public MetadataToken ReadMemberRefParent() => ReadCodedIndex(CodedIndex.MemberRefParent);
        public MetadataToken ReadHasSemantics() => ReadCodedIndex(CodedIndex.HasSemantics);
        public MetadataToken ReadMethodDefOrRef() => ReadCodedIndex(CodedIndex.MethodDefOrRef);
        public MetadataToken ReadMemberForwarded() => ReadCodedIndex(CodedIndex.MemberForwarded);
        public MetadataToken ReadImplementation() => ReadCodedIndex(CodedIndex.Implementation);
        public MetadataToken ReadCustomAttributeType() => ReadCodedIndex(CodedIndex.CustomAttributeType);
        public MetadataToken ReadResolutionScope() => ReadCodedIndex(CodedIndex.ResolutionScope);
        public MetadataToken ReadTypeOrMethodDef() => ReadCodedIndex(CodedIndex.TypeOrMethodDef);

        public void Skip(int bytes)
        {
            reader.BaseStream.Seek(bytes, SeekOrigin.Current);
        }

        public long Position
        {
            get => reader.BaseStream.Position;
            set => reader.BaseStream.Seek(value, SeekOrigin.Begin);
        }
    }
}