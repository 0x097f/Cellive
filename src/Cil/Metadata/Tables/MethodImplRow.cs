namespace CilDotNet.Cil.Metadata.Tables
{
    public sealed class MethodImplRow
    {
        public uint Class { get; set; }
        public uint MethodBody { get; set; }
        public uint MethodDeclaration { get; set; }

        public MetadataToken ClassToken { get; set; }
        public MetadataToken MethodBodyToken { get; set; }
        public MetadataToken MethodDeclarationToken { get; set; }

        public override string ToString() =>
            $"MethodImpl {ClassToken} Body={MethodBodyToken} Decl={MethodDeclarationToken}";
    }
}