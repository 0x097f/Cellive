namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class ClassLayoutRow
    {
        public ushort PackingSize { get; set; }
        public uint ClassSize { get; set; }
        public uint Parent { get; set; }

        public MetadataToken ParentToken { get; set; }

        public override string ToString() =>
            $"ClassLayout {ParentToken} Pack={PackingSize} Size={ClassSize}";
    }
}