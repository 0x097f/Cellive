namespace CilDotNet.Cil.Metadata.Signatures
{
    public sealed class FieldSignature
    {
        public bool IsInstance { get; set; }
        public TypeSignature? FieldType { get; set; }

        public override string ToString() => FieldType?.FullName ?? "?";
    }
}