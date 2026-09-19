namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class ModuleRow
    {
        public ushort Generation { get; set; }
        public uint NameIndex { get; set; }
        public uint MvidIndex { get; set; }
        public uint EncIdIndex { get; set; }
        public uint EncBaseIdIndex { get; set; }

        public string Name { get; set; } = string.Empty;
        public System.Guid Mvid { get; set; }
        public System.Guid EncId { get; set; }
        public System.Guid EncBaseId { get; set; }

        public override string ToString() => Name;
    }
}