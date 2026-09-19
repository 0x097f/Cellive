namespace CilDotNet.Cil.Metadata.Signatures
{
    public enum SignatureKind : byte
    {
        Default = 0x00,
        C = 0x01,
        StdCall = 0x02,
        ThisCall = 0x03,
        FastCall = 0x04,
        VarArg = 0x05,
        Field = 0x06,
        LocalVar = 0x07,
        Property = 0x08,
        GenericInst = 0x0A,
    }
}