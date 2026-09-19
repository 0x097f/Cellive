namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class EventMapRow
    {
        public uint Parent { get; set; }
        public uint EventList { get; set; }

        public MetadataToken ParentToken { get; set; }
        public MetadataToken EventListToken { get; set; }

        public override string ToString() =>
            $"EventMap {ParentToken} -> {EventListToken}";
    }
}