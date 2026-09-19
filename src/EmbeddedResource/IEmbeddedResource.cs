using System.Collections.Generic;

namespace CilDotNet.EmbeddedResource
{
    public interface IEmbeddedResource
    {
        string Name { get; }
        byte[] Data { get; }
        int Size { get; }
    }
}