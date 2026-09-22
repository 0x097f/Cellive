using System.Collections.Generic;

namespace Cellive.EmbeddedResource
{
    public interface IEmbeddedResourceReader
    {
        IReadOnlyList<IEmbeddedResource> ReadAll();
        IEmbeddedResource? Read(string name);
        IReadOnlyList<string> ListNames();
    }
}