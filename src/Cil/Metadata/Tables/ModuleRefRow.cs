namespace Cellive.Cil.Metadata.Tables
{
    public sealed class ModuleRefRow
    {
        public uint NameIndex { get; set; }
        public string Name { get; set; } = string.Empty;

        public override string ToString() => Name;
    }
}