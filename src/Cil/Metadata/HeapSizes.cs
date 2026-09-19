namespace CilDotNet.Cil.Metadata
{
    [Flags]
    public enum HeapSizes : byte
    {
        None = 0x00,
        StringHeapLarge = 0x01,
        GuidHeapLarge = 0x02,
        BlobHeapLarge = 0x04,
        ExtraData = 0x40,
        HasDelete = 0x80,
    }
}