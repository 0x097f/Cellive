namespace CilDotNet.Executable
{
    /// <summary>
    /// Reference: CorHdr.h
    /// Only for .net assembly.
    /// </summary>
  
    [Flags]
    public enum CorFlags : uint
    {
        /// <summary>
        /// assembly contains only IL code (and uhh probably no any native code).
        /// </summary>
        COMIMAGE_FLAGS_ILONLY = 0x00000001,

        /// <summary>
        /// assembly can only be loaded into a 32-bit process.
        /// </summary>
        COMIMAGE_FLAGS_32BITREQUIRED = 0x00000002,

        /// <summary>
        /// assembly is a library and contains only IL code.
        /// </summary>
        COMIMAGE_FLAGS_IL_LIBRARY = 0x00000004,

        /// <summary>
        /// assembly has a strong name signature.
        /// </summary>
        COMIMAGE_FLAGS_STRONGNAMESIGNED = 0x00000008,

        /// <summary>
        /// entry point is  native (unmanaged) address.
        /// </summary>
        COMIMAGE_FLAGS_NATIVE_ENTRYPOINT = 0x00000010,

        /// <summary>
        /// Debugging data is tracked for the assembly.
        /// </summary>
        COMIMAGE_FLAGS_TRACKDEBUGDATA = 0x00010000,

        /// <summary>
        /// The assembly prefers to be loaded into a 32b process, even on 64b systems.
        /// used for AnyCPU 32-bit preferred images.
        /// </summary>
        COMIMAGE_FLAGS_32BITPREFERRED = 0x00020000,
    }
    //ThE eNd wOW
}