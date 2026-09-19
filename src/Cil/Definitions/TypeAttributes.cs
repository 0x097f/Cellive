namespace CilDotNet.Cil.Definitions
{
    /// <summary>
    ///Type attributes
    ///Reference:https://learn.microsoft.com/dotnet/framework/unmanaged-api/metadata/cortypeattr-enumeration
    /// </summary>
    [Flags]
    public enum TypeAttributes
    {
        VisibilityMask = 0x00000007,

        //Not public (internal/private)
        NotPublic = 0x00000000,

        //Public
        Public = 0x00000001,

        //public
        NestedPublic = 0x00000002,

        //privare
        NestedPrivate = 0x00000003,

        //protected family
        NestedFamily = 0x00000004,

        //internal
        NestedAssembly = 0x00000005,

        //protected internal
        NestedFamANDAssem = 0x00000006,
        NestedFamORAssem = 0x00000007,

        LayoutMask = 0x00000018,
        AutoLayout = 0x00000000,
        SequentialLayout = 0x00000008,
        ExplicitLayout = 0x00000010,
        //(Extended) Add in .NET7+
        ExtendedLayout        = 0x00000018, 
        
        ClassSemanticsMask = 0x00000020,
        Class = 0x00000000,

        //Interface
        Interface = 0x00000020,

        //Special semantics

        Abstract = 0x00000080,
        Sealed = 0x00000100,

        SpecialName = 0x00000400,
        RTSpecialName = 0x00000800,

        Import = 0x00001000,
        [Obsolete("This flag is obsolete.Please use System.SerializableAttribute attribute.")]
        Serializable = 0x00002000,

        WindowsRuntime = 0x00004000,

        StringFormatMask = 0x00030000,

        AnsiClass = 0x00000000,
        UnicodeClass = 0x00010000,
        AutoClass = 0x00020000,

        CustomFormatClass = 0x00030000,
        CustomFormatMask = 0x00C00000,

        //.cctor
        BeforeFieldInit = 0x00100000,
        Forwarder = 0x00200000,

        //Reserved flags
        ReservedMask = 0x00040800,
        HasSecurity = 0x00040000,
    }
}