namespace Cellive.Cil.Metadata.Tables
{
    public sealed class DeclSecurityRow
    {
        public ushort Action { get; set; }
        public uint Parent { get; set; }
        public uint PermissionSetIndex { get; set; }

        public byte[] PermissionSet { get; set; } = System.Array.Empty<byte>();
        public MetadataToken ParentToken { get; set; }

        public override string ToString() =>
            $"DeclSecurity Action=0x{Action:X4} Parent={ParentToken} ({PermissionSet.Length} bytes)";
    }
}