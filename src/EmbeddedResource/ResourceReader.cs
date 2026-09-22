namespace Cellive.EmbeddedResource
{
    public sealed class ResourceReader : IEmbeddedResourceReader, IDisposable
    {
        private readonly PEReader peReader;
        private readonly MetadataReader metadataReader;
        private bool disposed;

        public ResourceReader(string assemblyPath)
        {
            var stream = File.OpenRead(assemblyPath);
            peReader = new PEReader(stream);
            metadataReader = peReader.GetMetadataReader();
        }

        public ResourceReader(Stream stream)
        {
            peReader = new PEReader(stream);
            metadataReader = peReader.GetMetadataReader();
        }

        public IReadOnlyList<IEmbeddedResource> ReadAll()
        {
            var result = new List<IEmbeddedResource>();

            foreach (var handle in metadataReader.ManifestResources)
            {
                var resource = metadataReader.GetManifestResource(handle);
                var name = metadataReader.GetString(resource.Name);

                var entry = new EmbeddedResourceEntry
                {
                    Name = name,
                };

                if (resource.Implementation.IsNil)
                {
                    //Embedded resource
                    try
                    {
                        var data = ReadEmbeddedData((uint)resource.Offset);
                        entry.Data = data;
                    }
                    catch
                    {
                        entry.Data = Array.Empty<byte>();
                    }
                }
                else
                {
                    entry.IsLinked = true;
                    entry.FileName = ResolveLinkedFile(resource.Implementation);
                }

                result.Add(entry);
            }

            return result;
        }

        public IEmbeddedResource? Read(string name)
        {
            foreach (var handle in metadataReader.ManifestResources)
            {
                var resource = metadataReader.GetManifestResource(handle);
                var resourceName = metadataReader.GetString(resource.Name);

                if (resourceName != name)
                    continue;

                var entry = new EmbeddedResourceEntry { Name = resourceName };

                if (resource.Implementation.IsNil)
                {
                    entry.Data = ReadEmbeddedData((uint)resource.Offset);
                }
                else
                {
                    entry.IsLinked = true;
                    entry.FileName = ResolveLinkedFile(resource.Implementation);
                }

                return entry;
            }

            return null;
        }

        public IReadOnlyList<string> ListNames()
        {
            var names = new List<string>();
            foreach (var handle in metadataReader.ManifestResources)
            {
                var resource = metadataReader.GetManifestResource(handle);
                names.Add(metadataReader.GetString(resource.Name));
            }
            return names;
        }

        private byte[] ReadEmbeddedData(uint offset)
        {
            var corHeader = peReader.PEHeaders.CorHeader;
            if (corHeader == null)
                return Array.Empty<byte>();

            var resourcesDir = corHeader.ResourcesDirectory;
            var sectionData = peReader.GetSectionData(resourcesDir.RelativeVirtualAddress);

            if (sectionData.Length == 0)
                return Array.Empty<byte>();

            int start = (int)offset;
            if (start < 0 || start + 4 > sectionData.Length)
                return Array.Empty<byte>();

            var reader = sectionData.GetReader(start, sectionData.Length - start);
            uint size = reader.ReadUInt32();

            if (size == 0 || size > int.MaxValue)
                return Array.Empty<byte>();

            return reader.ReadBytes((int)size);
        }

        private string? ResolveLinkedFile(EntityHandle implementation)
        {
            try
            {
                if (implementation.Kind == HandleKind.AssemblyFile)
                {
                    var file = metadataReader.GetAssemblyFile((AssemblyFileHandle)implementation);
                    return metadataReader.GetString(file.Name);
                }
            }
            catch
            {
            }
            return null;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                peReader?.Dispose();
                disposed = true;
            }
        }
    }
}