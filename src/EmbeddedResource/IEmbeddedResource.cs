using System.Collections.Generic;

namespace Cellive.EmbeddedResource
{
    public interface IEmbeddedResource
    {
        string Name { get; }
        byte[] Data { get; }
        int Size { get; }
    }
}